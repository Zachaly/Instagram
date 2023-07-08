using FluentValidation;
using Instagram.Application.Abstraction;
using Instagram.Models.Response;
using Instagram.Models.UserClaim;
using Instagram.Models.UserClaim.Request;

namespace Instagram.Api.Infrastructure.ServiceProxy
{
    public interface IUserClaimServiceProxy : IUserClaimService { }

    public class UserClaimServiceProxy : HttpLoggingKeylessServiceProxyBase<UserClaimModel, GetUserClaimRequest, IUserClaimService>, IUserClaimServiceProxy
    {
        private readonly IResponseFactory _responseFactory;
        private readonly IValidator<AddUserClaimRequest> _addValidator;

        public UserClaimServiceProxy(ILogger<IUserClaimService> logger, IHttpContextAccessor httpContextAccessor,
            IUserClaimService service, IResponseFactory responseFactory, IValidator<AddUserClaimRequest> addValidator) : base(logger, httpContextAccessor, service)
        {
            _responseFactory = responseFactory;
            _addValidator = addValidator;
        }

        public async Task<ResponseModel> AddAsync(AddUserClaimRequest request)
        {
            LogInformation("Add");

            var validation = _addValidator.Validate(request);

            var response = validation.IsValid ? await _service.AddAsync(request) : _responseFactory.CreateValidationError(validation);

            LogResponse(response, "Add");

            return response;
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
