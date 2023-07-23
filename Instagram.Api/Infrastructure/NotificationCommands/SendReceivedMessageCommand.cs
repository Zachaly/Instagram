using Instagram.Api.Hubs;
using Instagram.Database.Repository;
using MediatR;
using Microsoft.AspNetCore.SignalR;

namespace Instagram.Api.Infrastructure.NotificationCommands
{
    public class SendReceivedMessageCommand : IRequest
    {
        public long MessageId { get; set; }
        public long ReceiverId { get; set; }
    }

    public class SendReceivedMessageHandler : IRequestHandler<SendReceivedMessageCommand>
    {
        private readonly IDirectMessageRepository _directMessageRepository;
        private readonly IHubContext<DirectMessageHub, IDirectMessageClient> _messageHub;

        public SendReceivedMessageHandler(IDirectMessageRepository directMessageRepository,
            IHubContext<DirectMessageHub, IDirectMessageClient> messageHub)
        {
            _directMessageRepository = directMessageRepository;
            _messageHub = messageHub;
        }

        public async Task Handle(SendReceivedMessageCommand request, CancellationToken cancellationToken)
        {
            var message = await _directMessageRepository.GetByIdAsync(request.MessageId);

            await _messageHub.Clients
                .Users(message.SenderId.ToString(), request.ReceiverId.ToString())
                .MessageReceived(message);
        }
    }
}
