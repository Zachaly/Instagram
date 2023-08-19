using FluentValidation;
using Instagram.Api.Infrastructure.ServiceProxy.Abstraction;
using Instagram.Application.Abstraction;
using Instagram.Models.Response;
using Instagram.Models.UserBan;
using Instagram.Models.UserBan.Request;

namespace Instagram.Api.Infrastructure.ServiceProxy
{
    public interface IUserBanServiceProxy : IUserBanService
    {
        
    }

    public class UserBanServiceProxy 
        : HttpLoggingServiceProxyBase<UserBanModel, GetUserBanRequest, AddUserBanRequest, IUserBanService>,
        IUserBanServiceProxy
    {
        public UserBanServiceProxy(ILogger<IUserBanService> logger, IHttpContextAccessor httpContextAccessor,
            IUserBanService userBanService, IResponseFactory responseFactory, IValidator<AddUserBanRequest> addValidator)
            : base(logger, httpContextAccessor, userBanService, responseFactory, addValidator)
        {
            ServiceName = "UserBan";
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
