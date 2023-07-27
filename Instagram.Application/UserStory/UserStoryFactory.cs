using Instagram.Application.Abstraction;
using Instagram.Domain.Entity;

namespace Instagram.Application
{
    internal class UserStoryFactory : IUserStoryFactory
    {
        public UserStoryImage Create(long userId, string fileName)
        {
            throw new NotImplementedException();
        }
    }
}
