using Instagram.Application.Abstraction;
using Instagram.Domain.Entity;
using Instagram.Models.UserBan.Request;

namespace Instagram.Application
{
    public class UserBanFactory : IUserBanFactory
    {
        public UserBan Create(AddUserBanRequest request)
            => new UserBan
            {
                EndDate = request.EndDate,
                StartDate = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds(),
                UserId = request.UserId,
            };
    }
}
