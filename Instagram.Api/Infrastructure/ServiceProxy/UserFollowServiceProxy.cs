using Instagram.Application.Abstraction;
using Instagram.Models.Response;
using Instagram.Models.UserFollow;
using Instagram.Models.UserFollow.Request;

namespace Instagram.Api.Infrastructure.ServiceProxy
{
    public interface IUserFollowServiceProxy : IUserFollowService { }

    public class UserFollowServiceProxy : HttpLoggingProxyBase<IUserFollowService>, IUserFollowServiceProxy
    {
        private readonly IUserFollowService _userFollowService;

        public UserFollowServiceProxy(ILogger<IUserFollowService> logger, IHttpContextAccessor httpContextAccessor,
            IUserFollowService userFollowService) : base(logger, httpContextAccessor)
        {
            _userFollowService = userFollowService;
        }

        public async Task<ResponseModel> AddAsync(AddUserFollowRequest request)
        {
            LogInformation("Add");

            var response = await _userFollowService.AddAsync(request);

            LogResponse(response, "Add");

            return response;
        }

        public async Task<ResponseModel> DeleteAsync(long followerId, long followedId)
        {
            LogInformation("Delete");

            var response = await _userFollowService.DeleteAsync(followerId, followedId);

            LogResponse(response, "Delete");

            return response;
        }

        public Task<IEnumerable<UserFollowModel>> GetAsync(GetUserFollowRequest request)
        {
            LogInformation("Get");

            return _userFollowService.GetAsync(request);
        }

        public Task<int> GetCountAsync(GetUserFollowRequest request)
        {
            LogInformation("Get Count");

            return _userFollowService.GetCountAsync(request);
        }
    }
}
