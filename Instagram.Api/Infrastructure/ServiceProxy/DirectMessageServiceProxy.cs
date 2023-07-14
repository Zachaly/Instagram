using FluentValidation;
using Instagram.Application.Abstraction;
using Instagram.Models.DirectMessage;
using Instagram.Models.DirectMessage.Request;
using Instagram.Models.Response;

namespace Instagram.Api.Infrastructure.ServiceProxy
{
    public interface IDirectMessageServiceProxy : IDirectMessageService { }

    public class DirectMessageServiceProxy : HttpLoggingServiceProxyBase<DirectMessageModel, GetDirectMessageRequest, IDirectMessageService>, IDirectMessageServiceProxy
    {
        private readonly IValidator<AddDirectMessageRequest> _addValidator;
        private readonly IValidator<UpdateDirectMessageRequest> _updateValidator;
        private readonly IResponseFactory _responseFactory;

        public DirectMessageServiceProxy(ILogger<IDirectMessageService> logger, IHttpContextAccessor httpContextAccessor,
            IDirectMessageService service, IResponseFactory responseFactory,
            IValidator<AddDirectMessageRequest> addValidator, IValidator<UpdateDirectMessageRequest> updateValidator) 
            : base(logger, httpContextAccessor, service)
        {
            _addValidator = addValidator;
            _updateValidator = updateValidator;
            _responseFactory = responseFactory;
        }

        public async Task<ResponseModel> AddAsync(AddDirectMessageRequest request)
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

        public async Task<ResponseModel> UpdateAsync(UpdateDirectMessageRequest request)
        {
            LogInformation("Update");

            var validation = _updateValidator.Validate(request);

            var response = validation.IsValid ? await _service.UpdateAsync(request) : _responseFactory.CreateValidationError(validation);

            LogResponse(response, "Update");

            return response;
        }
    }
}
