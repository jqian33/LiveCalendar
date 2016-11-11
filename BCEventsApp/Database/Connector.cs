using System;
using System.Data.SqlClient;
using System.Threading.Tasks;
using System.Web.Configuration;
using BCEventsApp.Interfaces.Database;

namespace BCEventsApp.Database
{
    public class Connector: IConnector
    {

        private readonly string connectionString;
        private readonly string dataSource = WebConfigurationManager.AppSettings["dataSource"];
        private readonly string initialCatalog = WebConfigurationManager.AppSettings["initialCatalog"];
        private readonly string adminId = WebConfigurationManager.AppSettings["adminId"];
        private readonly string pwd = WebConfigurationManager.AppSettings["pwd"];
        
        public Connector()
        {
            // ReSharper disable once UseStringInterpolation
            connectionString = string.Format("Data Source={0};Initial Catalog={1};User id={2};Password={3};", dataSource, initialCatalog, adminId, pwd);
        }

        public async Task<SqlConnection> OpenConnection()
        {
            var connection = new SqlConnection(connectionString);
            try
            {
                await connection.OpenAsync();
            }
            catch (Exception e)
            {
                string errorMessage = e.ToString();
//                 Log the error
            }
            return connection;
        }

        public void CloseConnection(SqlConnection connection)
        {
            connection.Close();
        }

    }

}