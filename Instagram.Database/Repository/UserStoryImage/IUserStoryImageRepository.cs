using Instagram.Database.Repository.Abstraction;
using Instagram.Domain.Entity;
using Instagram.Models.UserStory;
using Instagram.Models.UserStory.Request;

namespace Instagram.Database.Repository
{
    public interface IUserStoryImageRepository : IRepositoryBase<UserStoryImage, UserStoryImageModel, GetUserStoryRequest>
    {
        Task<IEnumerable<UserStoryModel>> GetStoriesAsync(GetUserStoryRequest request);
    }
}
