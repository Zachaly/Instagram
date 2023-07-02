using Instagram.Application.Abstraction;
using Instagram.Models.Response;
using Instagram.Models.UserFollow;
using Instagram.Models.UserFollow.Request;

namespace Instagram.Api.Infrastructure.ServiceProxy
{
    public interface IUserFollowServiceProxy : IUserFollowService { }

    public class UserFollowServiceProxy : HttpLoggingKeylessServiceProxyBase<UserFollowModel, GetUserFollowRequest, IUserFollowService>, IUserFollowServiceProxy
    {
        public UserFollowServiceProxy(ILogger<IUserFollowService> logger, IHttpContextAccessor httpContextAccessor,
            IUserFollowService userFollowService) : base(logger, httpContextAccessor, userFollowService)
        {
        }

        public async Task<ResponseModel> AddAsync(AddUserFollowRequest request)
        {
            LogInformation("Add");

            var response = await _service.AddAsync(request);

            LogResponse(response, "Add");

            return response;
        }

        public async Task<ResponseModel> DeleteAsync(long followerId, long followedId)
        {
            LogInformation("Delete");

            var response = await _service.DeleteAsync(followerId, followedId);

            LogResponse(response, "Delete");

            return response;
        }
    }
}
