using Instagram.Application.Abstraction;
using Instagram.Domain.Entity;
using Instagram.Models.UserBlock.Request;

namespace Instagram.Application
{
    public class UserBlockFactory : IUserBlockFactory
    {
        public UserBlock Create(AddUserBlockRequest request)
            => new UserBlock
            {
                BlockedUserId = request.BlockedUserId,
                BlockingUserId = request.BlockingUserId,
            };
    }
}
