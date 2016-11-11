using System.Data.SqlClient;
using System.Threading.Tasks;

namespace BCEventsApp.Interfaces.Database
{
    public interface IConnector
    {
        Task<SqlConnection> OpenConnection();

        void CloseConnection(SqlConnection connection);

    }
}
