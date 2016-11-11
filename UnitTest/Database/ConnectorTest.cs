using System.Data;
using System.Threading.Tasks;
using BCEventsApp.Database;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace UnitTest.Database
{
    [TestClass]
    public class ConnectorTest
    {
        [TestMethod]
        public async Task TestDbConnection()
        {
            var connector = new Connector();
            var connection = await connector.OpenConnection();
            Assert.IsTrue(connection.State == ConnectionState.Open);
            connector.CloseConnection(connection);
            Assert.IsTrue(connection.State == ConnectionState.Closed);
        }

        [TestMethod]
        public async Task TestMultipleDbConnections()
        {
            var connector = new Connector();
            
            var connection1 = await connector.OpenConnection();
            var connection2 = await connector.OpenConnection();

            Assert.IsTrue(connection1.State == ConnectionState.Open);
            Assert.IsTrue(connection1.State == ConnectionState.Open);
            connector.CloseConnection(connection1);
            connector.CloseConnection(connection2);
        }
    }
}
