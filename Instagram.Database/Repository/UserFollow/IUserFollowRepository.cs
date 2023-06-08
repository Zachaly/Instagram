using Instagram.Database.Repository.Abstraction;
using Instagram.Domain.Entity;
using Instagram.Models.UserFollow;
using Instagram.Models.UserFollow.Request;

namespace Instagram.Database.Repository
{
    public interface IUserFollowRepository : IKeylessRepositoryBase<UserFollow, UserFollowModel, GetUserFollowRequest>
    {
        Task DeleteAsync(long followerId, long followedId);
    }
}
