using Instagram.Domain.Entity;
using Instagram.Models.UserBlock.Request;

namespace Instagram.Application.Abstraction
{
    public interface IUserBlockFactory
    {
        UserBlock Create(AddUserBlockRequest request);
    }
}
