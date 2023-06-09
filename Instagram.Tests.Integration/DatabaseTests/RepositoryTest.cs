using Dapper;
using Instagram.Database.Factory;
using Moq;
using System.Data.SqlClient;

namespace Instagram.Tests.Integration.DatabaseTests
{
    public class RepositoryTest : IDisposable
    {
        protected readonly IConnectionFactory _connectionFactory;
        private readonly string[] _truncateQueries =
        {
            "TRUNCATE TABLE [User]",
            "TRUNCATE TABLE [Post]",
            "TRUNCATE TABLE [PostImage]",
            "TRUNCATE TABLE [UserFollow]"
        };

        public RepositoryTest()
        {
            var factoryMock =  new Mock<IConnectionFactory>();
            factoryMock.Setup(x => x.CreateConnection()).Returns(new SqlConnection(Constants.ConnectionString));
            _connectionFactory = factoryMock.Object;
        }

        public IEnumerable<T> GetFromDatabase<T>(string query, object param = null)
        {
            using(var connection = new SqlConnection(Constants.ConnectionString))
            {
                return connection.Query<T>(query, param);
            }
        }

        public void ExecuteQuery(string query, object param)
        {
            using(var connection = new SqlConnection(Constants.ConnectionString))
            {
                connection.Query(query, param);
            }
        }

        public void Dispose()
        {
            using (var connection = new SqlConnection(Constants.ConnectionString))
            {
                foreach (var query in _truncateQueries)
                {
                    connection.Query(query);
                }
            }
        }
    }
}
