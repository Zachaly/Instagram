using Instagram.Models.Response;
using Instagram.Models.UserBlock;
using Instagram.Models.UserBlock.Request;

namespace Instagram.Application.Abstraction
{
    public interface IUserBlockService : IServiceBase<UserBlockModel, GetUserBlockRequest>
    {
        Task<ResponseModel> AddAsync(AddUserBlockRequest request);
        Task<ResponseModel> DeleteByIdAsync(long id);
    }
}
