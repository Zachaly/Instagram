using FluentValidation;
using Instagram.Api.Infrastructure.ServiceProxy.Abstraction;
using Instagram.Application.Abstraction;
using Instagram.Models.Notification;
using Instagram.Models.Notification.Request;
using Instagram.Models.Response;

namespace Instagram.Api.Infrastructure.ServiceProxy
{
    public interface INotificationServiceProxy : INotificationService { }

    public class NotificationServiceProxy 
        : HttpLoggingServiceProxyBase<NotificationModel, GetNotificationRequest, AddNotificationRequest, INotificationService>,
        INotificationServiceProxy
    {
        private readonly IValidator<UpdateNotificationRequest> _updateValidator;

        public NotificationServiceProxy(ILogger<INotificationService> logger, IHttpContextAccessor httpContextAccessor,
            INotificationService service, IValidator<AddNotificationRequest> addValidator,
            IValidator<UpdateNotificationRequest> updateValidator, IResponseFactory responseFactory) 
            : base(logger, httpContextAccessor, service, responseFactory, addValidator)
        {
            _updateValidator = updateValidator;
            ServiceName = "Notification";
        }

        public async Task<ResponseModel> DeleteByIdAsync(long id)
        {
            LogInformation("Delete By Id");

            var response = await _service.DeleteByIdAsync(id);

            LogResponse(response, "Delete By Id");

            return response;
        }

        public async Task<ResponseModel> UpdateAsync(UpdateNotificationRequest request)
        {
            LogInformation("Update");

            var validation = _updateValidator.Validate(request);

            var response = validation.IsValid ? await _service.UpdateAsync(request) : _responseFactory.CreateValidationError(validation);

            LogResponse(response, "Update");

            return response;
        }
    }
}
