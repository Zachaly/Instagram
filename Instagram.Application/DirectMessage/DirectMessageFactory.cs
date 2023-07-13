using Instagram.Application.Abstraction;
using Instagram.Domain.Entity;
using Instagram.Models.DirectMessage.Request;

namespace Instagram.Application
{
    public class DirectMessageFactory : IDirectMessageFactory
    {
        public DirectMessage Create(AddDirectMessageRequest request)
            => new DirectMessage
            {
                Content = request.Content,
                SenderId = request.SenderId,
                Read = false,
                ReceiverId = request.ReceiverId,
                Created = DateTimeOffset.Now.ToUnixTimeMilliseconds(),
            };
    }
}
