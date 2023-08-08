using Instagram.Application;
using Instagram.Models.UserBlock.Request;

namespace Instagram.Tests.Unit.FactoryTests
{
    public class UserBlockFactoryTests
    {
        private readonly UserBlockFactory _factory;

        public UserBlockFactoryTests()
        {
            _factory = new UserBlockFactory();
        }

        [Fact]
        public void Create_CreatesValidEntity()
        {
            var request = new AddUserBlockRequest
            {
                BlockedUserId = 1,
                BlockingUserId = 2,
            };

            var block = _factory.Create(request);

            Assert.Equal(request.BlockingUserId, block.BlockingUserId);
            Assert.Equal(request.BlockedUserId, block.BlockedUserId);
        }
    }
}
