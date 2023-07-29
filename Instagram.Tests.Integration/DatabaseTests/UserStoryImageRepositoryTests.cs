using Instagram.Database.Repository;
using Instagram.Database.Sql;
using Instagram.Domain.Entity;
using Instagram.Models.UserStory.Request;

namespace Instagram.Tests.Integration.DatabaseTests
{
    public class UserStoryImageRepositoryTests : RepositoryTest
    {
        private readonly UserStoryImageRepository _repository;

        public UserStoryImageRepositoryTests()
        {
            _repository = new UserStoryImageRepository(new SqlQueryBuilder(), _connectionFactory);
        }

        [Fact]
        public async Task GetStoriesAsync_ModelMappedCorrectly()
        {
            Insert("User", FakeDataFactory.GenerateUsers(2));

            var users = GetFromDatabase<User>("SELECT * FROM [User]");

            var firstUser = users.First();
            var lastUser = users.Last();

            Insert("UserStoryImage", FakeDataFactory.GenerateStoryImages(firstUser.Id, 5));
            Insert("UserStoryImage", FakeDataFactory.GenerateStoryImages(lastUser.Id, 5));

            var images = GetFromDatabase<UserStoryImage>("SELECT * FROM UserStoryImage");

            var res = await _repository.GetStoriesAsync(new GetUserStoryRequest());

            Assert.Equal(users.Count(), res.Count());
            Assert.Equivalent(res.Select(x => x.UserId), users.Select(x => x.Id));
            Assert.All(res.Where(x => x.UserId == firstUser.Id), story =>
            {
                Assert.Equal(firstUser.Nickname, story.UserName);
                Assert.Equivalent(images.Where(x => x.UserId == firstUser.Id).Select(x => x.Id), story.Images.Select(x => x.Id));
            });
            Assert.All(res.Where(x => x.UserId == lastUser.Id), story =>
            {
                Assert.Equal(lastUser.Nickname, story.UserName);
                Assert.Equivalent(images.Where(x => x.UserId == lastUser.Id).Select(x => x.Id), story.Images.Select(x => x.Id));
            });
        }
    }
}
