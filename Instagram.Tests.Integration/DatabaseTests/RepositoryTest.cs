using Instagram.Database.Factory;
using NSubstitute;
using System.Data.SqlClient;

namespace Instagram.Tests.Integration.DatabaseTests
{
    public class RepositoryTest : DatabaseTest
    {
        protected readonly IConnectionFactory _connectionFactory;
        public RepositoryTest()
        {
            var factoryMock = Substitute.For<IConnectionFactory>();

            factoryMock.CreateConnection().Returns(new SqlConnection(Constants.ConnectionString));

            _connectionFactory = factoryMock;
        }
    }
}
