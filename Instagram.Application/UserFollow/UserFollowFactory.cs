using Instagram.Application.Abstraction;
using Instagram.Domain.Entity;
using Instagram.Models.UserFollow.Request;

namespace Instagram.Application
{
    public class UserFollowFactory : IUserFollowFactory
    {
        public UserFollow Create(AddUserFollowRequest request)
            => new UserFollow
            {
                FollowedUserId = request.FollowedUserId,
                FollowingUserId = request.FollowingUserId,
            };
    }
}
