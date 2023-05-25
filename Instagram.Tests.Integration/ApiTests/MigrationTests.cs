using Dapper;
using System.Data.SqlClient;

namespace Instagram.Tests.Integration.ApiTests
{
    public class MigrationTests : ApiTest
    {
        [Fact]
        public void ApplicationStart_DatabaseCreated()
        {
            List<string> dbNames;

            using(var connection = new SqlConnection(Constants.MasterConnection))
            {
                dbNames = connection.Query<string>("SELECT name FROM sys.databases").ToList();
            }

            Assert.Contains(dbNames, x => x == DatabaseName);
        }
    }
}
