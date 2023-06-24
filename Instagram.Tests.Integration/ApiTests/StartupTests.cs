using Dapper;
using Instagram.Api.Authorization;
using Instagram.Domain.Entity;
using Instagram.Tests.Integration.ApiTests.Infrastructure;
using System.Data.SqlClient;

namespace Instagram.Tests.Integration.ApiTests
{
    public class StartupTests : ApiTest
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

        [Fact]
        public void ApplicationStart_AdminCreated()
        {
            var userClaims = GetFromDatabase<UserClaim>("SELECT * FROM [UserClaim] WHERE [Value]=@Value", new { Value = UserClaimValues.Admin });
            
            var users = GetFromDatabase<User>("SELECT * FROM [User] WHERE [Id] IN @Ids", new { Ids = userClaims.Select(x => x.UserId) });

            Assert.NotEmpty(users);
            Assert.NotEmpty(userClaims);
        }
    }
}
