using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Threading.Tasks;
using BCEventsApp.Interfaces.Database;

namespace BCEventsApp.Database
{
    public class EventActions: IEventActions
    {

        private readonly IConnector connector;

        public EventActions(IConnector connector)
        {
            this.connector = connector;
        }

        public async Task<List<int>> GetTagIds (int eventId)
        {
            var idList = new List<int>();
            var connection = await connector.OpenConnection();
            if (connection.State == ConnectionState.Open)
            {
                var cmd = new SqlCommand("dbo.GetEventTagID", connection);
                cmd.Parameters.Add("@eventID", SqlDbType.VarChar).Value = eventId;
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

        public async Task<string> GetOwnerId (int eventId)
        {
            string id = null;
            var connection = await connector.OpenConnection();
            if (connection.State == ConnectionState.Open)
            {
                var cmd = new SqlCommand("dbo.GetEventOwner", connection);
                cmd.Parameters.Add("@eventID", SqlDbType.VarChar).Value = eventId;
                cmd.CommandType = CommandType.StoredProcedure;

                await cmd.ExecuteNonQueryAsync();
                var reader = cmd.ExecuteReader();

                id = reader.Read() ? reader.GetValue(0).ToString() : null;

                reader.Close();
                connector.CloseConnection(connection);
            }
            return id;
        }

        public async Task<List<string>> GetAttendeeIds(int eventId)
        {
            var idList = new List<string>();
            var connection = await connector.OpenConnection();
            if (connection.State == ConnectionState.Open)
            {
                var cmd = new SqlCommand("dbo.GetEventAttendeeID", connection);
                cmd.Parameters.Add("@eventID", SqlDbType.VarChar).Value = eventId;
                cmd.CommandType = CommandType.StoredProcedure;

                await cmd.ExecuteNonQueryAsync();
                var reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    idList.Add(reader.GetValue(0).ToString());
                }

                reader.Close();
                connector.CloseConnection(connection);
            }
            return idList;   
        }

        public async Task InsertTag(int eventId, string tagName)
        {
            var connection = await connector.OpenConnection();
            if (connection.State == ConnectionState.Open)
            {
                var cmd = new SqlCommand("dbo.EventAddTag", connection);
                cmd.Parameters.Add("@eventID", SqlDbType.VarChar).Value = eventId;
                cmd.Parameters.Add("@tagName", SqlDbType.VarChar).Value = tagName;
                cmd.CommandType = CommandType.StoredProcedure;

                await cmd.ExecuteNonQueryAsync();
            }
            connector.CloseConnection(connection);
        }

        public async Task RemoveTag(int eventId, int tagId)
        {
            var connection = await connector.OpenConnection();
            if (connection.State == ConnectionState.Open)
            {
                var cmd = new SqlCommand("dbo.RemoveEventTag", connection);
                cmd.Parameters.Add("@eventID", SqlDbType.VarChar).Value = eventId;
                cmd.Parameters.Add("@tagId", SqlDbType.VarChar).Value = tagId;
                cmd.CommandType = CommandType.StoredProcedure;

                await cmd.ExecuteNonQueryAsync();
                connector.CloseConnection(connection);
            }
        }

    }
}