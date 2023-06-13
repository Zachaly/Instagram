using Dapper;
using Instagram.Database.Factory;
using Instagram.Database.Sql;
using Instagram.Domain.Entity;
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
            "TRUNCATE TABLE [UserFollow]",
            "TRUNCATE TABLE [PostLike]"
        };

        public RepositoryTest()
        {
            var factoryMock =  new Mock<IConnectionFactory>();
            factoryMock.Setup(x => x.CreateConnection()).Returns(new SqlConnection(Constants.ConnectionString));
            _connectionFactory = factoryMock.Object;
        }

        protected IEnumerable<T> GetFromDatabase<T>(string query, object param = null)
        {
            using(var connection = new SqlConnection(Constants.ConnectionString))
            {
                return connection.Query<T>(query, param);
            }
        }

        protected void ExecuteQuery(string query, object param)
        {
            using(var connection = new SqlConnection(Constants.ConnectionString))
            {
                connection.Query(query, param);
            }
        }

        protected void Insert<T>(string table, T item) where T : IEntity
        {
            var query = new SqlQueryBuilder().BuildInsert(table, item).Build();

            ExecuteQuery(query, item);
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
