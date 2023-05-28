using Instagram.Models.Response;
using Instagram.Models.User;
using Instagram.Models.User.Request;

namespace Instagram.Application.Abstraction
{
    public interface IUserService
    {
        Task<IEnumerable<UserModel>> GetAsync(GetUserRequest request);
        Task<UserModel> GetByIdAsync(long id);
        Task<ResponseModel> UpdateAsync(UpdateUserRequest request);
    }
}
