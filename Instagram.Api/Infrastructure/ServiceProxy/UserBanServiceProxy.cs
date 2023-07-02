using Instagram.Application.Abstraction;
using Instagram.Models.Response;
using Instagram.Models.UserBan;
using Instagram.Models.UserBan.Request;

namespace Instagram.Api.Infrastructure.ServiceProxy
{
    public interface IUserBanServiceProxy : IUserBanService
    {
        
    }

    public class UserBanServiceProxy : HttpLoggingServiceProxyBase<UserBanModel, GetUserBanRequest, IUserBanService>, IUserBanServiceProxy
    {
        public UserBanServiceProxy(ILogger<IUserBanService> logger, IHttpContextAccessor httpContextAccessor,
            IUserBanService userBanService) : base(logger, httpContextAccessor, userBanService)
        {
        }

        public async Task<ResponseModel> AddAsync(AddUserBanRequest request)
        {
            LogInformation("Add");

            var response = await _service.AddAsync(request);

            LogResponse(response, "Add");

            return response;
        }

        public async Task<ResponseModel> DeleteAsync(long id)
        {
            LogInformation("Delete");

            var response = await _service.DeleteAsync(id);

            LogResponse(response, "Delete");

            return response;
        }
    }
}
