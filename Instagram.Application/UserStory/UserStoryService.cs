using Instagram.Application.Abstraction;
using Instagram.Database.Repository;
using Instagram.Models.UserStory;
using Instagram.Models.UserStory.Request;

namespace Instagram.Application.UserStory
{
    public class UserStoryService : IUserStoryService
    {
        private readonly IUserStoryImageRepository _repository;

        public UserStoryService(IUserStoryImageRepository repository)
        {
            _repository = repository;
        }

        public Task<IEnumerable<UserStoryModel>> GetAsync(GetUserStoryRequest request)
        {
            throw new NotImplementedException();
        }
    }
}
