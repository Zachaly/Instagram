using Instagram.Database.Repository;
using Instagram.Database.Sql;
using Instagram.Domain.Entity;
using Instagram.Models.UserClaim.Request;

namespace Instagram.Tests.Integration.DatabaseTests
{
    public class UserClaimRepositoryTests : RepositoryTest
    {
        private readonly UserClaimRepository _repository;

        public UserClaimRepositoryTests() : base()
        {
            _repository = new UserClaimRepository(new SqlQueryBuilder(), _connectionFactory);
        }

        [Fact]
        public async Task GetEntitiesAsync_ReturnEntities()
        {
            var claims = FakeDataFactory.GenerateUserClaims(1, 2);
            claims.AddRange(FakeDataFactory.GenerateUserClaims(2, 2));
            claims.AddRange(FakeDataFactory.GenerateUserClaims(3, 2));

            Insert("UserClaim", claims);

            var res = await _repository.GetEntitiesAsync(new GetUserClaimRequest());

            Assert.Equal(claims.Count, res.Count());
            Assert.All(res, claim =>
            {
                Assert.Contains(claims, c => c.Value == claim.Value && c.UserId == claim.UserId);
            });
        }

        [Fact]
        public async Task DeleteClaimAsync_DeletesProperClaim()
        {
            var claimsToInsert = FakeDataFactory.GenerateUserClaims(1, 2);
            claimsToInsert.AddRange(FakeDataFactory.GenerateUserClaims(2, 2));
            claimsToInsert.AddRange(FakeDataFactory.GenerateUserClaims(3, 2));

            Insert("UserClaim", claimsToInsert);

            var claimToDelete = GetFromDatabase<UserClaim>("SELECT * FROM [UserClaim]").Last();

            await _repository.DeleteAsync(claimToDelete.UserId, claimToDelete.Value);

            var claims = GetFromDatabase<UserClaim>("SELECT * FROM [UserClaim]");

            Assert.DoesNotContain(claims, c => c.UserId == claimToDelete.UserId && c.Value == claimToDelete.Value);
        }
    }
}
