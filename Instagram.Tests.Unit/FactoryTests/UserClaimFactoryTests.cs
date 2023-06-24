using Instagram.Application;
using Instagram.Models.UserClaim.Request;

namespace Instagram.Tests.Unit.FactoryTests
{
    public class UserClaimFactoryTests
    {
        private readonly UserClaimFactory _factory;

        public UserClaimFactoryTests()
        {
            _factory = new UserClaimFactory();
        }

        [Fact]
        public void Create_CreatesValidEntity()
        {
            var request = new AddUserClaimRequest
            {
                UserId = 1,
                Value = "claim"
            };

            var claim = _factory.Create(request);

            Assert.Equal(request.Value, claim.Value);
            Assert.Equal(request.UserId, claim.UserId);
        }
    }
}
