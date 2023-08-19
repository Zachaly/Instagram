using Instagram.Models.Response;
using Instagram.Models.UserBan;
using Instagram.Models.UserBan.Request;

namespace Instagram.Application.Abstraction
{
    public interface IUserBanService : IServiceBase<UserBanModel, GetUserBanRequest, AddUserBanRequest>
    {
        Task<ResponseModel> DeleteAsync(long id);
    }
}
