using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Threading.Tasks;
using BCEventsApp.Exceptions;
using BCEventsApp.Interfaces.Database;
using BCEventsApp.Models;

namespace BCEventsApp.Database
{
    public class GlobalActions: IGlobalActions
    {
        private readonly IConnector connector;

        public GlobalActions(IConnector connector)
        {
            this.connector = connector;
        }
        
        public async Task DeleteUser(string username)
        {
            var connection = await connector.OpenConnection();
            if (connection.State == ConnectionState.Open)
            {
                SqlCommand cmd = new SqlCommand("dbo.DeleteUser", connection);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@uid", SqlDbType.VarChar).Value = username;
                await cmd.ExecuteNonQueryAsync();
                connector.CloseConnection(connection);
            }
        }

        public async Task AddUser(User user, string hash)
        {
            var username = user.Id.ToLower();
            var firstName = user.FirstName;
            var lastname = user.LastName;
            var passwordHash = hash;

            var connection = await connector.OpenConnection();
            if (connection.State == ConnectionState.Open)
            {
                SqlCommand cmd = new SqlCommand("dbo.AddUser", connection);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@uid", SqlDbType.VarChar).Value = username;
                cmd.Parameters.Add("@lastname", SqlDbType.VarChar).Value = lastname;
                cmd.Parameters.Add("@firstname", SqlDbType.VarChar).Value = firstName;
                cmd.Parameters.Add("@passwordHash", SqlDbType.VarChar).Value = passwordHash;
                var returnParameter = cmd.Parameters.Add("@ReturnVal", SqlDbType.Int);
                returnParameter.Direction = ParameterDirection.ReturnValue;

                await cmd.ExecuteNonQueryAsync();

                if ((int)returnParameter.Value == -1)
                {
                    throw CustomException.UserExistException();
                }

                connector.CloseConnection(connection);
            }
        }

        public async Task<Dictionary<int, Calendar>> GetAllCalendars()
        {
            var calendarList = new Dictionary<int, Calendar>();
            var connection = await connector.OpenConnection();
            if (connection.State == ConnectionState.Open)
            {
                var cmd = new SqlCommand("dbo.GetAllCalendars", connection);
                cmd.CommandType = CommandType.StoredProcedure;
                await cmd.ExecuteNonQueryAsync();
                var reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    var calendar = new Calendar()
                    {
                        Id = reader.GetInt32(0),
                        Title = reader.GetValue(1).ToString()
                    };
                    if (!reader.IsDBNull(2))
                    {
                        calendar.Description = reader.GetValue(2).ToString();
                    }
                    calendarList.Add(calendar.Id, calendar);
                }
                reader.Close();
                connector.CloseConnection(connection);
            }
            return calendarList;
        }

        public async Task<Calendar> GetCalendarById(int calendarId)
        {
            var calendar = new Calendar();
            var connection = await connector.OpenConnection();
            if (connection.State == ConnectionState.Open)
            {
                var cmd = new SqlCommand("dbo.GetCalendar", connection);
                cmd.Parameters.Add("@calendarID", SqlDbType.VarChar).Value = calendarId;
                cmd.CommandType = CommandType.StoredProcedure;
                await cmd.ExecuteNonQueryAsync();
                var reader = cmd.ExecuteReader();
                if (reader.Read())
                {
                    calendar.Id = reader.GetInt32(0);
                    calendar.Title = reader.GetValue(1).ToString();
                    if (!reader.IsDBNull(2))
                    {
                        calendar.Description = reader.GetValue(2).ToString();
                    }
                }
                else
                {
                    calendar = null;
                }
                reader.Close();
                connector.CloseConnection(connection);
            }
            return calendar;
        }

        public async Task<List<int>> GetPublishedCalendarIds()
        {
            var eventList = new List<int>();
            var connection = await connector.OpenConnection();
            if (connection.State == ConnectionState.Open)
            {
                var cmd = new SqlCommand("dbo.GetPublishedCalendars", connection);
                cmd.CommandType = CommandType.StoredProcedure;
                await cmd.ExecuteNonQueryAsync();
                var reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    var id = reader.GetInt32(0);
                    eventList.Add(id);
                }
                reader.Close();
                connector.CloseConnection(connection);
            }
            return eventList;
        }

        public async Task<Dictionary<string, User>> GetAllUsers()
        {
            var userList = new Dictionary<string, User>();
            var connection = await connector.OpenConnection();
            if (connection.State == ConnectionState.Open)
            {
                var cmd = new SqlCommand("dbo.GetAllUsers", connection);
                cmd.CommandType = CommandType.StoredProcedure;
                await cmd.ExecuteNonQueryAsync();
                var reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    var user = new User()
                    {
                        Id = reader.GetValue(0).ToString(),
                        FirstName = reader.GetValue(1).ToString(),
                        LastName = reader.GetValue(2).ToString()

                    };
                    userList.Add(user.Id, user);
                }
                reader.Close();
                connector.CloseConnection(connection);
            }
            return userList;
        }

        public async Task<Dictionary<int, Tag>> GetAllTags()
        {
            var allTags = new Dictionary<int, Tag>();
            var connection = await connector.OpenConnection();
            if (connection.State == ConnectionState.Open)
            {
                var cmd = new SqlCommand("dbo.GetAllTags", connection);
                cmd.CommandType = CommandType.StoredProcedure;

                await cmd.ExecuteNonQueryAsync();
                var reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    var tag = new Tag()
                    {
                        Id = reader.GetInt32(0),
                        Name = reader.GetValue(1).ToString()
                    };
                    allTags.Add(tag.Id, tag);
                }

                reader.Close();
                connector.CloseConnection(connection);
            }
            return allTags;
        }

        public async Task<Dictionary<int, Event>> GetAllEvents()
        {
            var eventList = new Dictionary<int, Event>();
            var connection = await connector.OpenConnection();
            if (connection.State == ConnectionState.Open)
            {
                var cmd = new SqlCommand("dbo.GetAllEvents", connection);
                cmd.CommandType = CommandType.StoredProcedure;
                await cmd.ExecuteNonQueryAsync();
                var reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    var currentEvent = new Event
                    {
                        Id = reader.GetInt32(0),
                        Title = reader.GetValue(1).ToString(),
                        Start = reader.GetDateTime(4),
                        Duration = reader.GetInt32(5)
                    };
                    if (!reader.IsDBNull(2))
                    {
                        currentEvent.Description = reader.GetValue(2).ToString();
                    }
                    if (!reader.IsDBNull(3))
                    {
                        currentEvent.Location = reader.GetValue(3).ToString();
                    }
                    if (!reader.IsDBNull(6))
                    {
                        currentEvent.End = Convert.ToDateTime(reader.GetDateTime(6));
                    }
                    if (!reader.IsDBNull(7))
                    {
                        currentEvent.RRule = reader.GetValue(7).ToString();
                    }
                    eventList.Add(currentEvent.Id, currentEvent);
                }
                reader.Close();
                connector.CloseConnection(connection);
            }
            return eventList;
        }

        public async Task<List<int>> GetPublishedEventIds()
        {
            var eventList = new List<int>();
            var connection = await connector.OpenConnection();
            if (connection.State == ConnectionState.Open)
            {
                var cmd = new SqlCommand("dbo.GetPublishedEvents", connection);
                cmd.CommandType = CommandType.StoredProcedure;
                await cmd.ExecuteNonQueryAsync();
                var reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    var id = reader.GetInt32(0);
                    eventList.Add(id);
                }
                reader.Close();
                connector.CloseConnection(connection);
            }
            return eventList;
        }

        public async Task<bool> VerifyPasswordHash(string userId, string passwordHash)
        {
            var verified = false;
            var connection = await connector.OpenConnection();
            if (connection.State == ConnectionState.Open)
            {
                var cmd = new SqlCommand("dbo.VerifyPasswordHash", connection);
                cmd.Parameters.Add("@userID", SqlDbType.VarChar).Value = userId;
                cmd.Parameters.Add("@passwordHash", SqlDbType.VarChar).Value = passwordHash;
                cmd.Parameters.Add("@verified", SqlDbType.Int).Direction = ParameterDirection.Output;

                cmd.CommandType = CommandType.StoredProcedure;

                await cmd.ExecuteNonQueryAsync();
                connector.CloseConnection(connection);
                verified = Convert.ToBoolean(cmd.Parameters["@verified"].Value);
            }
            return verified;
        }

    }
}