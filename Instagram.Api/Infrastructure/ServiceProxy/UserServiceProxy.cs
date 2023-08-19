using FluentValidation;
using Instagram.Api.Infrastructure.ServiceProxy.Abstraction;
using Instagram.Application.Abstraction;
using Instagram.Models.Response;
using Instagram.Models.User;
using Instagram.Models.User.Request;

namespace Instagram.Api.Infrastructure.ServiceProxy
{
    public interface IUserServiceProxy : IUserService { }

    public class UserServiceProxy : HttpLoggingServiceProxyBase<UserModel, GetUserRequest, IUserService>, IUserServiceProxy
    {
        private readonly IResponseFactory _responseFactory;
        private readonly IValidator<UpdateUserRequest> _updateValidator;

        public UserServiceProxy(ILogger<IUserService> logger, IHttpContextAccessor httpContextAccessor,
            IUserService userService, IResponseFactory responseFactory, IValidator<UpdateUserRequest> updateValidator)
            : base(logger, httpContextAccessor, userService)
        {
            _responseFactory = responseFactory;
            _updateValidator = updateValidator;
            ServiceName = "User";
        }

        public async Task<ResponseModel> UpdateAsync(UpdateUserRequest request)
        {
            LogInformation("Update");

            var validation = _updateValidator.Validate(request);

            var response = validation.IsValid ? await _service.UpdateAsync(request) : _responseFactory.CreateValidationError(validation);

            LogResponse(response, "Update");

            return response;
        }
    }
}
