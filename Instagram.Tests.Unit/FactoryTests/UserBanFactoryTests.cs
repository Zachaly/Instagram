using Instagram.Application;
using Instagram.Models.UserBan.Request;

namespace Instagram.Tests.Unit.FactoryTests
{
    public class UserBanFactoryTests
    {
        private readonly UserBanFactory _factory;

        public UserBanFactoryTests()
        {
            _factory = new UserBanFactory();
        }

        [Fact]
        public void Create_CreatesValidEntity()
        {
            var request = new AddUserBanRequest { UserId = 1, EndDate = 2137 };

            var ban = _factory.Create(request);

            Assert.Equal(request.UserId, ban.UserId);
            Assert.Equal(request.EndDate, ban.EndDate);
            Assert.NotEqual(default, ban.StartDate);
        }
    }
}
