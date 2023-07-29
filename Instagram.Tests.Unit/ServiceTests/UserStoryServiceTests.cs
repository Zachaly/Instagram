using Instagram.Application.UserStory;
using Instagram.Database.Repository;
using Instagram.Models.UserStory;
using Instagram.Models.UserStory.Request;
using Moq;

namespace Instagram.Tests.Unit.ServiceTests
{
    public class UserStoryServiceTests
    {
        private readonly Mock<IUserStoryImageRepository> _userStoryRepository;
        private readonly UserStoryService _service;

        public UserStoryServiceTests()
        {
            _userStoryRepository = new Mock<IUserStoryImageRepository>();

            _service = new UserStoryService(_userStoryRepository.Object);
        }

        [Fact]
        public async Task GetAsync_ReturnsStories()
        {
            var stories = new List<UserStoryModel>
            {
                new UserStoryModel { UserId = 1, },
                new UserStoryModel { UserId = 2, },
                new UserStoryModel { UserId = 3, },
                new UserStoryModel { UserId = 4, },
            };

            _userStoryRepository.Setup(x => x.GetStoriesAsync(It.IsAny<GetUserStoryRequest>()))
                .ReturnsAsync(stories);

            var res = await _service.GetAsync(new GetUserStoryRequest());

            Assert.Equivalent(stories.Select(x => x.UserId), res.Select(x => x.UserId));
        }
    }
}
