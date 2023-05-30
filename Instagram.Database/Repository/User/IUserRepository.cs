using Instagram.Database.Repository.Abstraction;
using Instagram.Domain.Entity;
using Instagram.Models.User;
using Instagram.Models.User.Request;

namespace Instagram.Database.Repository
{
    public interface IUserRepository : IRepositoryBase<User, UserModel, GetUserRequest>
    {
        Task<User> GetEntityByEmailAsync(string email);
        Task UpdateAsync(UpdateUserRequest request);
    }
}
