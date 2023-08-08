using Instagram.Database.Repository.Abstraction;
using Instagram.Domain.Entity;
using Instagram.Models.UserBlock;
using Instagram.Models.UserBlock.Request;

namespace Instagram.Database.Repository
{
    public interface IUserBlockRepository : IRepositoryBase<UserBlock, UserBlockModel, GetUserBlockRequest>
    {
    }
}
