using Instagram.Application.Abstraction;
using Instagram.Models.Response;
using Instagram.Models.User;
using Instagram.Models.User.Request;

namespace Instagram.Api.Infrastructure.ServiceProxy
{
    public interface IUserServiceProxy : IUserService { }

    public class UserServiceProxy : HttpLoggingServiceProxyBase<UserModel, GetUserRequest, IUserService>, IUserServiceProxy
    {
        public UserServiceProxy(ILogger<IUserService> logger, IHttpContextAccessor httpContextAccessor,
            IUserService userService) : base(logger, httpContextAccessor, userService)
        {
        }

        public async Task<ResponseModel> UpdateAsync(UpdateUserRequest request)
        {
            LogInformation("Update");

            var response = await _service.UpdateAsync(request);

            LogResponse(response, "Update");

            return response;
        }
    }
}
