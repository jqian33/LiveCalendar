using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Threading.Tasks;
using BCEventsApp.Interfaces.Database;

namespace BCEventsApp.Database
{
    public class CalendarActions: ICalendarActions
    {
        private readonly IConnector connector;

        public CalendarActions(IConnector connector)
        {
            this.connector = connector;
        }

        public async Task<List<int>> GetTagIds(int calendarId)
        {
            var idList = new List<int>();
            var connection = await connector.OpenConnection();
            if (connection.State == ConnectionState.Open)
            {
                var cmd = new SqlCommand("dbo.GetCalendarTagID", connection);
                cmd.Parameters.Add("@calendarID", SqlDbType.VarChar).Value = calendarId;
                cmd.CommandType = CommandType.StoredProcedure;

                await cmd.ExecuteNonQueryAsync();
                var reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    idList.Add(reader.GetInt32(0));
                }

                reader.Close();
                connector.CloseConnection(connection);
            }
            return idList;
        }

        public async Task<List<int>> GetEventIds(int calendarId)
        {
            var idList = new List<int>();
            var connection = await connector.OpenConnection();
            if (connection.State == ConnectionState.Open)
            {
                var cmd = new SqlCommand("dbo.GetCalendarEventID", connection);
                cmd.Parameters.Add("@calendarID", SqlDbType.VarChar).Value = calendarId;
                cmd.CommandType = CommandType.StoredProcedure;

                await cmd.ExecuteNonQueryAsync();
                var reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    idList.Add(reader.GetInt32(0));
                }

                reader.Close();
                connector.CloseConnection(connection);
            }
            return idList;
        }

        public async Task<string> GetOwnerId(int calendarId)
        {
            string id = null;
            var connection = await connector.OpenConnection();
            if (connection.State == ConnectionState.Open)
            {
                var cmd = new SqlCommand("dbo.GetCalendarOwner", connection);
                cmd.Parameters.Add("@calendarID", SqlDbType.VarChar).Value = calendarId;
                cmd.CommandType = CommandType.StoredProcedure;

                await cmd.ExecuteNonQueryAsync();
                var reader = cmd.ExecuteReader();
                id = reader.Read() ? reader.GetValue(0).ToString() : null;

                reader.Close();
                connector.CloseConnection(connection);
            }
            return id;
        }

        public async Task InsertTag(int calendarId, string tagName)
        {
            var connection = await connector.OpenConnection();
            if (connection.State == ConnectionState.Open)
            {
                var cmd = new SqlCommand("dbo.CalendarAddTag", connection);
                cmd.Parameters.Add("@calendarID", SqlDbType.VarChar).Value = calendarId;
                cmd.Parameters.Add("@tagName", SqlDbType.VarChar).Value = tagName;
                cmd.CommandType = CommandType.StoredProcedure;

                await cmd.ExecuteNonQueryAsync();

                connector.CloseConnection(connection);
            }
        }

        public async Task RemoveTag(int calendarId, int tagId)
        {
            var connection = await connector.OpenConnection();
            if (connection.State == ConnectionState.Open)
            {
                var cmd = new SqlCommand("dbo.RemoveCalendarTag", connection);
                cmd.Parameters.Add("@calendarID", SqlDbType.VarChar).Value = calendarId;
                cmd.Parameters.Add("@tagID", SqlDbType.VarChar).Value = tagId;
                cmd.CommandType = CommandType.StoredProcedure;

                await cmd.ExecuteNonQueryAsync();
                connector.CloseConnection(connection);
            }
        }
    }
}