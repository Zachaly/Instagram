using Instagram.Domain.Entity;
using Instagram.Models.UserFollow.Request;

namespace Instagram.Application.Abstraction
{
    public interface IUserFollowFactory
    {
        UserFollow Create(AddUserFollowRequest request);
    }
}
