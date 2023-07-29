using Instagram.Domain.Entity;

namespace Instagram.Application.Abstraction
{
    public interface IUserStoryFactory
    {
        UserStoryImage Create(long userId, string fileName);
    }
}
