using Instagram.Api.Hubs;
using Instagram.Api.Infrastructure.ServiceProxy;
using Instagram.Application.Abstraction;
using Instagram.Database.Repository;
using Instagram.Models.Notification.Request;
using MediatR;
using Microsoft.AspNetCore.SignalR;

namespace Instagram.Api.Infrastructure.NotificationCommands
{
    public class SendFollowNotificationCommand : IRequest
    {
        public long FollowerId { get; set; }
        public long FollowedUserId { get; set; }
    }

    public class SendFollowNotificationHandler : IRequestHandler<SendFollowNotificationCommand>
    {
        private readonly IUserRepository _userRepository;
        private readonly INotificationService _notificationService;
        private readonly IHubContext<NotificationHub, NotificationClient> _notificationHub;

        public SendFollowNotificationHandler(IUserRepository userRepository, INotificationService notificationService,
            IHubContext<NotificationHub, NotificationClient> notificationHub)
        {
            _userRepository = userRepository;
            _notificationService = notificationService;
            _notificationHub = notificationHub;
        }

        public async Task Handle(SendFollowNotificationCommand request, CancellationToken cancellationToken)
        {
            var follower = await _userRepository.GetByIdAsync(request.FollowerId);
            var addNotificationRequest = new AddNotificationRequest
            {
                Message = $"{follower.Nickname} started following you",
                UserId = request.FollowedUserId
            };

            var response = await _notificationService.AddAsync(addNotificationRequest);

            await _notificationHub.Clients.Client(request.FollowedUserId.ToString())
                .NotificationReceived(await _notificationService.GetByIdAsync(response.NewEntityId.Value));
        }
    }
}
