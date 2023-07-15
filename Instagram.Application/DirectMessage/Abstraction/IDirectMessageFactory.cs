using Instagram.Domain.Entity;
using Instagram.Models.DirectMessage.Request;

namespace Instagram.Application.Abstraction
{
    public interface IDirectMessageFactory
    {
        DirectMessage Create(AddDirectMessageRequest request);
    }
}
