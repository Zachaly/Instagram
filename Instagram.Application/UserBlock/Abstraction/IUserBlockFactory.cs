using Instagram.Domain.Entity;
using Instagram.Models.UserBlock.Request;

namespace Instagram.Application.Abstraction
{
    public interface IUserBlockFactory : IEntityFactory<UserBlock, AddUserBlockRequest>
    {

    }
}
