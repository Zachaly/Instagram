using Instagram.Application.Abstraction;
using Instagram.Domain.Entity;

namespace Instagram.Application
{
    public class UserStoryFactory : IUserStoryFactory
    {
        public UserStoryImage Create(long userId, string fileName)
            => new UserStoryImage
            {
                Created = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds(),
                UserId = userId,
                FileName = fileName
            };
    }
}
