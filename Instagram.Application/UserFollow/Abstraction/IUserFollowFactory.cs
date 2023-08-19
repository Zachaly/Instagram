using Instagram.Domain.Entity;
using Instagram.Models.UserFollow.Request;

namespace Instagram.Application.Abstraction
{
    public interface IUserFollowFactory : IEntityFactory<UserFollow, AddUserFollowRequest>
    {
    }
}
