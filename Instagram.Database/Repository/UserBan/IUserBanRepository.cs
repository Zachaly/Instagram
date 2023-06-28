using Instagram.Database.Repository.Abstraction;
using Instagram.Domain.Entity;
using Instagram.Models.UserBan;
using Instagram.Models.UserBan.Request;

namespace Instagram.Database.Repository
{
    public interface IUserBanRepository : IRepositoryBase<UserBan, UserBanModel, GetUserBanRequest>
    {
    }
}
