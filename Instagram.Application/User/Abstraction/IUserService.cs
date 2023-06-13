using Instagram.Models.Response;
using Instagram.Models.User;
using Instagram.Models.User.Request;

namespace Instagram.Application.Abstraction
{
    public interface IUserService : IServiceBase<UserModel, GetUserRequest>
    {
        Task<ResponseModel> UpdateAsync(UpdateUserRequest request);
    }
}
