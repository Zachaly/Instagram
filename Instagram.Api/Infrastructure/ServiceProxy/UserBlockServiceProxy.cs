using FluentValidation;
using Instagram.Application.Abstraction;
using Instagram.Models.Response;
using Instagram.Models.UserBlock;
using Instagram.Models.UserBlock.Request;

namespace Instagram.Api.Infrastructure.ServiceProxy
{
    public interface IUserBlockServiceProxy : IUserBlockService
    {

    }

    public class UserBlockServiceProxy : HttpLoggingServiceProxyBase<UserBlockModel, GetUserBlockRequest, IUserBlockService>, IUserBlockServiceProxy
    {
        private readonly IValidator<AddUserBlockRequest> _addValidator;
        private readonly IResponseFactory _responseFactory;

        public UserBlockServiceProxy(ILogger<IUserBlockService> logger, IHttpContextAccessor httpContextAccessor, IUserBlockService service,
            IValidator<AddUserBlockRequest> addValidator, IResponseFactory responseFactory) : base(logger, httpContextAccessor, service)
        {
            _addValidator = addValidator;
            _responseFactory = responseFactory;
        }

        public async Task<ResponseModel> AddAsync(AddUserBlockRequest request)
        {
            LogInformation("Add");

            var validation = _addValidator.Validate(request);

            var response = validation.IsValid ? await _service.AddAsync(request) : _responseFactory.CreateValidationError(validation);

            LogResponse(response, "Add");

            return response;
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
