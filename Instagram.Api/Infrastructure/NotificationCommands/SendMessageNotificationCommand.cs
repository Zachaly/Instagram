using Instagram.Api.Hubs;
using Instagram.Application.Abstraction;
using Instagram.Models.Notification.Request;
using MediatR;
using Microsoft.AspNetCore.SignalR;

namespace Instagram.Api.Infrastructure.NotificationCommands
{
    public class SendMessageNotificationCommand : IRequest
    {
        public long SenderId { get; set; }
        public long ReceiverId { get; set; }
    }

    public class SendMessageNotificationHandler : IRequestHandler<SendMessageNotificationCommand>
    {
        private readonly INotificationService _notificationService;
        private readonly IUserService _userService;
        private readonly IHubContext<NotificationHub, NotificationClient> _notificationHub;

        public SendMessageNotificationHandler(INotificationService notificationService, IUserService userService, 
            IHubContext<NotificationHub, NotificationClient> notificatioHub)
        {
            _notificationService = notificationService;
            _userService = userService;
            _notificationHub = notificatioHub;
        }

        public async Task Handle(SendMessageNotificationCommand request, CancellationToken cancellationToken)
        {
            var sender = await _userService.GetByIdAsync(request.SenderId);

            var addNotificationRequest = new AddNotificationRequest
            {
                UserId = request.ReceiverId,
                Message = $"{sender.Nickname} send you a message"
            };

            var response = await _notificationService.AddAsync(addNotificationRequest);

            await _notificationHub.Clients.Client(request.ReceiverId.ToString())
                .NotificationReceived(await _notificationService.GetByIdAsync(response.NewEntityId.Value));
        }
    }
}
