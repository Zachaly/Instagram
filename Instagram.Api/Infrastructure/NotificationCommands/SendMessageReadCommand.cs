using Instagram.Api.Hubs;
using Instagram.Database.Repository;
using MediatR;
using Microsoft.AspNetCore.SignalR;

namespace Instagram.Api.Infrastructure.NotificationCommands
{
    public class SendMessageReadCommand : IRequest
    {
        public long MessageId { get; set; }
    }

    public class SendMessageReadHandler : IRequestHandler<SendMessageReadCommand>
    {
        private readonly IDirectMessageRepository _directMessageRepository;
        private readonly IHubContext<DirectMessageHub, IDirectMessageClient> _messageHub;

        public SendMessageReadHandler(IDirectMessageRepository directMessageRepository, 
            IHubContext<DirectMessageHub, IDirectMessageClient> messageHub)
        {
            _directMessageRepository = directMessageRepository;
            _messageHub = messageHub;
        }

        public async Task Handle(SendMessageReadCommand request, CancellationToken cancellationToken)
        {
            var message = await _directMessageRepository.GetEntityByIdAsync(request.MessageId);

            await _messageHub.Clients
                .Users(message.ReceiverId.ToString(), message.SenderId.ToString())
                .MessageRead(message.Id, message.Read);
        }
    }
}
