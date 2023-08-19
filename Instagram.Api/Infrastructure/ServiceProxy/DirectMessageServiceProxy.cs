using FluentValidation;
using Instagram.Api.Infrastructure.NotificationCommands;
using Instagram.Api.Infrastructure.ServiceProxy.Abstraction;
using Instagram.Application.Abstraction;
using Instagram.Models.DirectMessage;
using Instagram.Models.DirectMessage.Request;
using Instagram.Models.Response;
using MediatR;

namespace Instagram.Api.Infrastructure.ServiceProxy
{
    public interface IDirectMessageServiceProxy : IDirectMessageService { }

    public class DirectMessageServiceProxy : 
        HttpLoggingServiceProxyBase<DirectMessageModel, GetDirectMessageRequest, AddDirectMessageRequest, IDirectMessageService>,
        IDirectMessageServiceProxy
    {
        private readonly IValidator<UpdateDirectMessageRequest> _updateValidator;
        private readonly IMediator _mediator;

        public DirectMessageServiceProxy(ILogger<IDirectMessageService> logger, IHttpContextAccessor httpContextAccessor,
            IDirectMessageService service, IResponseFactory responseFactory,
            IValidator<AddDirectMessageRequest> addValidator, IValidator<UpdateDirectMessageRequest> updateValidator, IMediator mediator) 
            : base(logger, httpContextAccessor, service, responseFactory, addValidator)
        {
            _updateValidator = updateValidator;
            _mediator = mediator;
            ServiceName = "AccountVerification";
        }

        public override async Task<ResponseModel> AddAsync(AddDirectMessageRequest request)
        {
            var response = await base.AddAsync(request);

            if (response.Success)
            {
                await _mediator.Send(new SendMessageNotificationCommand
                {
                    ReceiverId = request.ReceiverId,
                    SenderId = request.SenderId,
                });

                await _mediator.Send(new SendReceivedMessageCommand
                {
                    ReceiverId = request.ReceiverId,
                    MessageId = response.NewEntityId!.Value
                });
            }

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

            if(response.Success)
            {
                await _mediator.Send(new SendMessageReadCommand
                {
                    MessageId = request.Id
                });
            }

            return response;
        }
    }
}
