using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Threading.Tasks;
using BCEventsApp.Exceptions;
using BCEventsApp.Interfaces.Database;

namespace BCEventsApp.Database
{
    public class UserActions: IUserActions
    {
        private readonly IConnector connector;

        public UserActions(IConnector connector)
        {
            this.connector = connector;
        }
        
        public async Task<List<int>> GetAllCalendars(string userId)
        {
            var idList = new List<int>();
            var connection = await connector.OpenConnection();
            if (connection.State == ConnectionState.Open)
            {
                var cmd = new SqlCommand("dbo.UserGetAllCalendars", connection);
                cmd.Parameters.Add("@userID", SqlDbType.VarChar).Value = userId;
                cmd.CommandType = CommandType.StoredProcedure;

                await cmd.ExecuteNonQueryAsync();
                var reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    var id = reader.GetInt32(0);
                    idList.Add(id);
                }

                reader.Close();
                connector.CloseConnection(connection);
            }
            return idList;
        }

        public async Task<int> GetPrimaryCalendarId(string userId)
        {
            int id = -1;
            var connection = await connector.OpenConnection();
            if (connection.State == ConnectionState.Open)
            {
                var cmd = new SqlCommand("dbo.GetUserPrimaryCalendarID", connection);
                cmd.Parameters.Add("@userID", SqlDbType.VarChar).Value = userId;
                cmd.CommandType = CommandType.StoredProcedure;

                await cmd.ExecuteNonQueryAsync();
                var reader = cmd.ExecuteReader();
                id = reader.Read() ? reader.GetInt32(0) : -1;
                connector.CloseConnection(connection);
            }
            return id;
        }

        public async Task<List<int>> GetAllSubscribedEvents(string userId)
        {
            var idList = new List<int>();
            var connection = await connector.OpenConnection();
            if (connection.State == ConnectionState.Open)
            {
                var cmd = new SqlCommand("dbo.UserGetAllSubscribedEvents", connection);
                cmd.Parameters.Add("@userID", SqlDbType.VarChar).Value = userId;
                cmd.CommandType = CommandType.StoredProcedure;

                await cmd.ExecuteNonQueryAsync();
                var reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    var id = reader.GetInt32(0);
                    idList.Add(id);
                }

                reader.Close();
                connector.CloseConnection(connection);
            }
            return idList;
        }

        public async Task SubscribeCalendar(string userId, int calendarId)
        {
            var connection = await connector.OpenConnection();
            if (connection.State == ConnectionState.Open)
            {
                var cmd = new SqlCommand("dbo.UserSubscribeCalendar", connection);
                cmd.Parameters.Add("@userID", SqlDbType.VarChar).Value = userId;
                cmd.Parameters.Add("@calendarID", SqlDbType.VarChar).Value = calendarId;
                cmd.CommandType = CommandType.StoredProcedure;

                try
                {
                    await cmd.ExecuteNonQueryAsync();
                }
                catch (SqlException)
                {
                    throw CustomException.CalendarSubscriptionException();
                }
                connector.CloseConnection(connection); 
            }    
        }

        public async Task UnsubscribeCalendar(string userId, int calendarId)
        {
            var connection = await connector.OpenConnection();
            if (connection.State == ConnectionState.Open)
            {
                var cmd = new SqlCommand("dbo.UserUnsubscribeCalendar", connection);
                cmd.Parameters.Add("@userID", SqlDbType.VarChar).Value = userId;
                cmd.Parameters.Add("@calendarID", SqlDbType.VarChar).Value = calendarId;
                cmd.CommandType = CommandType.StoredProcedure;

                await cmd.ExecuteNonQueryAsync();
                connector.CloseConnection(connection);
            }
        }

        public async Task SubscribeEvent(string userId, int eventId)
        {
            var connection = await connector.OpenConnection();
            if (connection.State == ConnectionState.Open)
            {
                var cmd = new SqlCommand("dbo.UserSubscribeEvent", connection);
                cmd.Parameters.Add("@userID", SqlDbType.VarChar).Value = userId;
                cmd.Parameters.Add("@eventID", SqlDbType.VarChar).Value = eventId;
                cmd.CommandType = CommandType.StoredProcedure;

                try
                {
                    await cmd.ExecuteNonQueryAsync();
                }
                catch (SqlException)
                {
                    throw CustomException.EventSubscriptionException();
                }
                connector.CloseConnection(connection);
            }
        }

        public async Task AttendEvent(string userId, int eventId)
        {
            var connection = await connector.OpenConnection();
            if (connection.State == ConnectionState.Open)
            {
                var cmd = new SqlCommand("dbo.UserAttendEvent", connection);
                cmd.Parameters.Add("@userID", SqlDbType.VarChar).Value = userId;
                cmd.Parameters.Add("@eventID", SqlDbType.VarChar).Value = eventId;
                cmd.CommandType = CommandType.StoredProcedure;

                try
                {
                    await cmd.ExecuteNonQueryAsync();
                }
                catch (SqlException)
                {
                    throw CustomException.AttendEventException();
                }
                connector.CloseConnection(connection);
            } 
        }

        public async Task WithdrawEvent(string userId, int eventId)
        {
            var connection = await connector.OpenConnection();
            if (connection.State == ConnectionState.Open)
            {
                var cmd = new SqlCommand("dbo.UserWithdrawEvent", connection);
                cmd.Parameters.Add("@userID", SqlDbType.VarChar).Value = userId;
                cmd.Parameters.Add("@eventID", SqlDbType.VarChar).Value = eventId;
                cmd.CommandType = CommandType.StoredProcedure;


                await cmd.ExecuteNonQueryAsync();

                connector.CloseConnection(connection);
            }
        }

        public async Task UnsubscribeEvent(string userId, int eventId)
        {
            var connection = await connector.OpenConnection();
            if (connection.State == ConnectionState.Open)
            {
                var cmd = new SqlCommand("dbo.UserUnsubscribeEvent", connection);
                cmd.Parameters.Add("@userID", SqlDbType.VarChar).Value = userId;
                cmd.Parameters.Add("@eventID", SqlDbType.VarChar).Value = eventId;
                cmd.CommandType = CommandType.StoredProcedure;

                await cmd.ExecuteNonQueryAsync();

                connector.CloseConnection(connection);
            }
        }

        public async Task<int> CreateCalendar(string userId, string title, string description)
        {
            var connection = await connector.OpenConnection();
            var id = -1;
            if (connection.State == ConnectionState.Open)
            {
                var cmd = new SqlCommand("dbo.UserCreateCalendar", connection);
                cmd.Parameters.Add("@userID", SqlDbType.VarChar).Value = userId;
                cmd.Parameters.Add("@title", SqlDbType.VarChar).Value = title;
                cmd.Parameters.Add("@calid", SqlDbType.Int).Direction = ParameterDirection.Output;
                if (description == null)
                {
                    cmd.Parameters.Add("@description", SqlDbType.VarChar).Value = DBNull.Value;
                }
                else
                {
                    cmd.Parameters.Add("@description", SqlDbType.VarChar).Value = description;
                }
                cmd.CommandType = CommandType.StoredProcedure;
                await cmd.ExecuteNonQueryAsync();
                id = Convert.ToInt32(cmd.Parameters["@calid"].Value);
                connector.CloseConnection(connection);
            }
            return id;
        }

        public async Task<int> CreateEvent(string userId, string title, string description, string location, DateTime startTime, int duration, DateTime endTime, string rRule, int calendarId)
        {
            var id = -1;
            var connection = await connector.OpenConnection();
            if (connection.State == ConnectionState.Open)
            {
                var cmd = new SqlCommand("dbo.UserCreateEvent", connection);
                cmd.Parameters.Add("@userID", SqlDbType.VarChar).Value = userId;
                cmd.Parameters.Add("@title", SqlDbType.VarChar).Value = title;
                cmd.Parameters.Add("@startDateTime", SqlDbType.VarChar).Value = startTime;
                cmd.Parameters.Add("@duration", SqlDbType.Int).Value = duration;
                cmd.Parameters.Add("@eid", SqlDbType.Int).Direction = ParameterDirection.Output;

                if (description == null)
                {
                    cmd.Parameters.Add("@description", SqlDbType.VarChar).Value = DBNull.Value;
                }
                else
                {
                    cmd.Parameters.Add("@description", SqlDbType.VarChar).Value = description;
                }
                if (location == null)
                {
                    cmd.Parameters.Add("@location", SqlDbType.VarChar).Value = DBNull.Value;
                }
                else
                {
                    cmd.Parameters.Add("@location", SqlDbType.VarChar).Value = location;
                }
                if (endTime == DateTime.MinValue)
                {
                    cmd.Parameters.Add("@endDateTime", SqlDbType.VarChar).Value = DBNull.Value;
                }
                else
                {
                    cmd.Parameters.Add("@endDateTime", SqlDbType.VarChar).Value = endTime;
                }
                if (calendarId == 0)
                {
                    cmd.Parameters.Add("@calendarID", SqlDbType.VarChar).Value = DBNull.Value;
                }
                else
                {
                    cmd.Parameters.Add("@calendarID", SqlDbType.VarChar).Value = calendarId;
                }
                if (rRule == null)
                {
                    cmd.Parameters.Add("@rRule", SqlDbType.VarChar).Value = DBNull.Value;
                }
                else
                {
                    cmd.Parameters.Add("@rRule", SqlDbType.VarChar).Value = rRule;
                }
                cmd.CommandType = CommandType.StoredProcedure;

                await cmd.ExecuteNonQueryAsync();
                id = Convert.ToInt32(cmd.Parameters["@eid"].Value);
                connector.CloseConnection(connection);
            }
            return id;
        }

        public async Task ModifyEventTitle(int eventId, string title)
        {
            if (title != null)
            {
                var connection = await connector.OpenConnection();
                if (connection.State == ConnectionState.Open)
                {
                    var cmd = new SqlCommand("dbo.ModifyEventTitle", connection);
                    cmd.Parameters.Add("@eventID", SqlDbType.VarChar).Value = eventId;
                    cmd.Parameters.Add("@title", SqlDbType.VarChar).Value = title;
                    cmd.CommandType = CommandType.StoredProcedure;

                    await cmd.ExecuteNonQueryAsync();
                    connector.CloseConnection(connection);
                }
            }
        }

        public async Task ModifyEventDescription(int eventId, string description)
        {
            var connection = await connector.OpenConnection();
            if (connection.State == ConnectionState.Open)
            {
                var cmd = new SqlCommand("dbo.ModifyEventDescription", connection);
                cmd.Parameters.Add("@eventID", SqlDbType.VarChar).Value = eventId;
                if (description == null)
                {
                    cmd.Parameters.Add("@description", SqlDbType.VarChar).Value = DBNull.Value;
                }
                else
                {
                    cmd.Parameters.Add("@description", SqlDbType.VarChar).Value = description;
                }
                cmd.CommandType = CommandType.StoredProcedure;

                await cmd.ExecuteNonQueryAsync();
                connector.CloseConnection(connection);
            }
        }

        public async Task ModifyEventLocation(int eventId, string location)
        {
            var connection = await connector.OpenConnection();
            if (connection.State == ConnectionState.Open)
            {
                var cmd = new SqlCommand("dbo.ModifyEventLocation", connection);
                cmd.Parameters.Add("@eventID", SqlDbType.VarChar).Value = eventId;
                if (location == null)
                {
                    cmd.Parameters.Add("@location", SqlDbType.VarChar).Value = DBNull.Value;
                }
                else
                {
                    cmd.Parameters.Add("@location", SqlDbType.VarChar).Value = location;
                }

                cmd.CommandType = CommandType.StoredProcedure;

                await cmd.ExecuteNonQueryAsync();
                connector.CloseConnection(connection);
            }
        }

        public async Task ModifyEventStartTime(int eventId, DateTime startTime)
        {
            if (startTime != DateTime.MinValue)
            {
                var connection = await connector.OpenConnection();
                if (connection.State == ConnectionState.Open)
                {
                    var cmd = new SqlCommand("dbo.ModifyEventStartDateTime", connection);
                    cmd.Parameters.Add("@eventID", SqlDbType.VarChar).Value = eventId;
                    cmd.Parameters.Add("@startDateTime", SqlDbType.VarChar).Value = startTime;
                    cmd.CommandType = CommandType.StoredProcedure;

                    await cmd.ExecuteNonQueryAsync();
                    connector.CloseConnection(connection);
                }
            }
        }

        public async Task ModifyEventDuration(int eventId, int duration)
        {
            if (duration != 0)
            {
                var connection = await connector.OpenConnection();
                if (connection.State == ConnectionState.Open)
                {
                    var cmd = new SqlCommand("dbo.ModifyEventDuration", connection);
                    cmd.Parameters.Add("@eventID", SqlDbType.VarChar).Value = eventId;
                    cmd.Parameters.Add("@duration", SqlDbType.VarChar).Value = duration;
                    cmd.CommandType = CommandType.StoredProcedure;

                    await cmd.ExecuteNonQueryAsync();
                    connector.CloseConnection(connection);
                }
            }
        }

        public async Task ModifyEventEndTime(int eventId, DateTime endTime)
        {
            var connection = await connector.OpenConnection();
            if (connection.State == ConnectionState.Open)
            {
                var cmd = new SqlCommand("dbo.ModifyEventEndDateTime", connection);
                cmd.Parameters.Add("@eventID", SqlDbType.VarChar).Value = eventId;
                if (endTime == DateTime.MinValue)
                {
                    cmd.Parameters.Add("@endDateTime", SqlDbType.VarChar).Value = DBNull.Value;
                }
                else
                {
                    cmd.Parameters.Add("@endDateTime", SqlDbType.VarChar).Value = endTime;
                }

                cmd.CommandType = CommandType.StoredProcedure;

                await cmd.ExecuteNonQueryAsync();
                connector.CloseConnection(connection);
            }
        }

        public async Task ModifyEventRRule(int eventId, string rRule)
        {
            var connection = await connector.OpenConnection();
            if (connection.State == ConnectionState.Open)
            {
                var cmd = new SqlCommand("dbo.ModifyEventRRule", connection);
                cmd.Parameters.Add("@eventID", SqlDbType.VarChar).Value = eventId;
                if (rRule == null)
                {
                    cmd.Parameters.Add("@rRule", SqlDbType.VarChar).Value = DBNull.Value;
                }
                else
                {
                    cmd.Parameters.Add("@rRule", SqlDbType.VarChar).Value = rRule;
                }

                cmd.CommandType = CommandType.StoredProcedure;

                await cmd.ExecuteNonQueryAsync();
                connector.CloseConnection(connection);
            }
        }

        public async Task PublishEvent(int eventId)
        {
            var connection = await connector.OpenConnection();
            if (connection.State == ConnectionState.Open)
            {
                var cmd = new SqlCommand("dbo.PublishEvent", connection);
                cmd.Parameters.Add("@eventID", SqlDbType.VarChar).Value = eventId;
                cmd.CommandType = CommandType.StoredProcedure;

                await cmd.ExecuteNonQueryAsync();
                connector.CloseConnection(connection);
            }
        }

        public async Task UnpublishEvent(int eventId)
        {
            var connection = await connector.OpenConnection();
            if (connection.State == ConnectionState.Open)
            {
                var cmd = new SqlCommand("dbo.UnpublishEvent", connection);
                cmd.Parameters.Add("@eventID", SqlDbType.VarChar).Value = eventId;
                cmd.CommandType = CommandType.StoredProcedure;

                await cmd.ExecuteNonQueryAsync();
                connector.CloseConnection(connection);
            }
        }

        public async Task DeleteEvent(int eventId)
        {
            var connection = await connector.OpenConnection();
            if (connection.State == ConnectionState.Open)
            {
                var cmd = new SqlCommand("dbo.DeleteEvent", connection);
                cmd.Parameters.Add("@eventID", SqlDbType.VarChar).Value = eventId;
                cmd.CommandType = CommandType.StoredProcedure;

                await cmd.ExecuteNonQueryAsync();
                connector.CloseConnection(connection);
            }
        }

        public async Task DeleteCalendar(int calendarId)
        {
            var connection = await connector.OpenConnection();
            if (connection.State == ConnectionState.Open)
            {
                var cmd = new SqlCommand("dbo.DeleteCalendar", connection);
                cmd.Parameters.Add("@calendarID", SqlDbType.VarChar).Value = calendarId;
                cmd.CommandType = CommandType.StoredProcedure;

                await cmd.ExecuteNonQueryAsync();
                connector.CloseConnection(connection);
            }
        }

        public async Task UnpublishCalendar(int calendarId)
        {
            var connection = await connector.OpenConnection();
            if (connection.State == ConnectionState.Open)
            {
                var cmd = new SqlCommand("dbo.UnPublishCalendar", connection);
                cmd.Parameters.Add("@calendarID", SqlDbType.VarChar).Value = calendarId;
                cmd.CommandType = CommandType.StoredProcedure;

                await cmd.ExecuteNonQueryAsync();
                connector.CloseConnection(connection);
            }
        }

        public async Task PublishCalendar(int calendarId)
        {
            var connection = await connector.OpenConnection();
            if (connection.State == ConnectionState.Open)
            {
                var cmd = new SqlCommand("dbo.PublishCalendar", connection);
                cmd.Parameters.Add("@calendarID", SqlDbType.VarChar).Value = calendarId;
                cmd.CommandType = CommandType.StoredProcedure;

                await cmd.ExecuteNonQueryAsync();
                connector.CloseConnection(connection);
            }
        }
    }
}