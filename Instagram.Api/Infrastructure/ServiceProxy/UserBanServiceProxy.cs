using FluentValidation;
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
        private readonly IResponseFactory _responseFactory;
        private readonly IValidator<AddUserBanRequest> _addValidator;

        public UserBanServiceProxy(ILogger<IUserBanService> logger, IHttpContextAccessor httpContextAccessor,
            IUserBanService userBanService, IResponseFactory responseFactory, IValidator<AddUserBanRequest> addValidator)
            : base(logger, httpContextAccessor, userBanService)
        {
            _responseFactory = responseFactory;
            _addValidator = addValidator;
        }

        public async Task<ResponseModel> AddAsync(AddUserBanRequest request)
        {
            LogInformation("Add");

            var validation = await _addValidator.ValidateAsync(request);

            var response = validation.IsValid ? await _service.AddAsync(request) : _responseFactory.CreateValidationError(validation);

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
