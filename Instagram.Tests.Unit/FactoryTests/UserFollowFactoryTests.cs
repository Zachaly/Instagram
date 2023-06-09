using Instagram.Application;
using Instagram.Models.UserFollow.Request;

namespace Instagram.Tests.Unit.FactoryTests
{
    public class UserFollowFactoryTests
    {
        private readonly UserFollowFactory _factory;

        public UserFollowFactoryTests()
        {
            _factory = new UserFollowFactory();
        }

        [Fact]
        public void Create_CreatesValidEntity()
        {
            var request = new AddUserFollowRequest
            {
                FollowedUserId = 1,
                FollowingUserId = 2,
            };

            var follow = _factory.Create(request);

            Assert.Equal(request.FollowingUserId, follow.FollowingUserId);
            Assert.Equal(request.FollowedUserId, follow.FollowedUserId);
        }
    }
}
