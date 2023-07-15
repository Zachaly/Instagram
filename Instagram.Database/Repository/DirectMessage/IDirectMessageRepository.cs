using Instagram.Database.Repository.Abstraction;
using Instagram.Domain.Entity;
using Instagram.Models.DirectMessage;
using Instagram.Models.DirectMessage.Request;

namespace Instagram.Database.Repository
{
    public interface IDirectMessageRepository : IRepositoryBase<DirectMessage, DirectMessageModel, GetDirectMessageRequest>
    {
        Task UpdateAsync(UpdateDirectMessageRequest request);
    }
}
