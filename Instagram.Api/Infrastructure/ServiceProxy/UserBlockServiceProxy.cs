using FluentValidation;
using Instagram.Api.Infrastructure.ServiceProxy.Abstraction;
using Instagram.Application.Abstraction;
using Instagram.Models.Response;
using Instagram.Models.UserBlock;
using Instagram.Models.UserBlock.Request;

namespace Instagram.Api.Infrastructure.ServiceProxy
{
    public interface IUserBlockServiceProxy : IUserBlockService
    {

    }

    public class UserBlockServiceProxy 
        : HttpLoggingServiceProxyBase<UserBlockModel, GetUserBlockRequest, AddUserBlockRequest, IUserBlockService>,
        IUserBlockServiceProxy
    {
        public UserBlockServiceProxy(ILogger<IUserBlockService> logger, IHttpContextAccessor httpContextAccessor, IUserBlockService service,
            IValidator<AddUserBlockRequest> addValidator, IResponseFactory responseFactory) 
            : base(logger, httpContextAccessor, service, responseFactory, addValidator)
        {
            ServiceName = "UserBlock";
        }

        public async Task<ResponseModel> DeleteByIdAsync(long id)
        {
            LogInformation("Delete By Id");

            var response = await _service.DeleteByIdAsync(id);

            LogResponse(response, "Delete By Id");

            return response;
        }
    }
}
