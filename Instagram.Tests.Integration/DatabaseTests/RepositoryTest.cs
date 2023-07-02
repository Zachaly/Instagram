using Instagram.Database.Factory;
using Moq;
using System.Data.SqlClient;

namespace Instagram.Tests.Integration.DatabaseTests
{
    public class RepositoryTest : DatabaseTest
    {
        protected readonly IConnectionFactory _connectionFactory;
        public RepositoryTest()
        {
            var factoryMock =  new Mock<IConnectionFactory>();
            factoryMock.Setup(x => x.CreateConnection()).Returns(new SqlConnection(Constants.ConnectionString));
            _connectionFactory = factoryMock.Object;
        }
    }
}
