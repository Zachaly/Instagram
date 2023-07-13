using Instagram.Application.Abstraction;
using Instagram.Domain.Entity;
using Instagram.Models.DirectMessage.Request;

namespace Instagram.Application
{
    public class DirectMessageFactory : IDirectMessageFactory
    {
        public DirectMessage Create(AddDirectMessageRequest request)
        {
            throw new NotImplementedException();
        }
    }
}
