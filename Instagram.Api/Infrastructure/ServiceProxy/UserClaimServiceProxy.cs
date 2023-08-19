using FluentValidation;
using Instagram.Api.Infrastructure.ServiceProxy.Abstraction;
using Instagram.Application.Abstraction;
using Instagram.Models.Response;
using Instagram.Models.UserClaim;
using Instagram.Models.UserClaim.Request;

namespace Instagram.Api.Infrastructure.ServiceProxy
{
    public interface IUserClaimServiceProxy : IUserClaimService { }

    public class UserClaimServiceProxy 
        : HttpLoggingKeylessServiceProxyBase<UserClaimModel, GetUserClaimRequest, AddUserClaimRequest, IUserClaimService>,
        IUserClaimServiceProxy
    {
        public UserClaimServiceProxy(ILogger<IUserClaimService> logger, IHttpContextAccessor httpContextAccessor,
            IUserClaimService service, IResponseFactory responseFactory, IValidator<AddUserClaimRequest> addValidator) 
            : base(logger, httpContextAccessor, service, responseFactory, addValidator)
        {
            ServiceName = "UserClaim";
        }

        public async Task<ResponseModel> DeleteAsync(long userId, string value)
        {
            LogInformation("Delete");

            var response = await _service.DeleteAsync(userId, value);

            LogResponse(response, "Delete");

            return response;
        }
    }
}
