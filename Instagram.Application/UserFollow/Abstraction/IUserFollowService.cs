using Instagram.Models.Response;
using Instagram.Models.UserFollow;
using Instagram.Models.UserFollow.Request;

namespace Instagram.Application.Abstraction
{
    public interface IUserFollowService : IKeylessServiceBase<UserFollowModel, GetUserFollowRequest, AddUserFollowRequest>
    {
        Task<ResponseModel> DeleteAsync(long followerId, long followedId);
    }
}
