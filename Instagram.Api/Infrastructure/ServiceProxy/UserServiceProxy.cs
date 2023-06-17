using Instagram.Application.Abstraction;
using Instagram.Models.Response;
using Instagram.Models.User;
using Instagram.Models.User.Request;

namespace Instagram.Api.Infrastructure.ServiceProxy
{
    public interface IUserServiceProxy : IUserService { }

    public class UserServiceProxy : HttpLoggingProxyBase<IUserService>, IUserServiceProxy
    {
        private readonly IUserService _userService;

        public UserServiceProxy(ILogger<IUserService> logger, IHttpContextAccessor httpContextAccessor,
            IUserService userService) : base(logger, httpContextAccessor)
        {
            _userService = userService;
        }

        public Task<IEnumerable<UserModel>> GetAsync(GetUserRequest request)
        {
            LogInformation("Get");

            return _userService.GetAsync(request);
        }

        public Task<UserModel> GetByIdAsync(long id)
        {
            LogInformation("Get By Id");

            return _userService.GetByIdAsync(id);
        }

        public Task<int> GetCountAsync(GetUserRequest request)
        {
            LogInformation("Get Count");

            return _userService.GetCountAsync(request);
        }

        public async Task<ResponseModel> UpdateAsync(UpdateUserRequest request)
        {
            LogInformation("Update");

            var response = await _userService.UpdateAsync(request);

            LogResponse(response, "Update");

            return response;
        }
    }
}
