using Instagram.Models.DirectMessage;
using Instagram.Models.DirectMessage.Request;
using Instagram.Models.Response;

namespace Instagram.Application.Abstraction
{
    public interface IDirectMessageService : IServiceBase<DirectMessageModel, GetDirectMessageRequest, AddDirectMessageRequest>
    {
        Task<ResponseModel> UpdateAsync(UpdateDirectMessageRequest request);
        Task<ResponseModel> DeleteByIdAsync(long id);
    }
}
