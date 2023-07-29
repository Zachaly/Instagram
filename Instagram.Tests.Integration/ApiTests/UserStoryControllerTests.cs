using Instagram.Domain.Entity;
using Instagram.Models.UserStory;
using Instagram.Tests.Integration.ApiTests.Infrastructure;
using System.Net.Http.Json;

namespace Instagram.Tests.Integration.ApiTests
{
    public class UserStoryControllerTests : ApiTest
    {
        const string Endpoint = "/api/user-story";

        [Fact]
        public async Task GetAsync_ReturnsProperData()
        {
            await Authorize();

            var users = GetFromDatabase<User>("SELECT * FROM [User]");

            var firstUser = users.First();
            var lastUser = users.Last();

            Insert("UserStoryImage", FakeDataFactory.GenerateStoryImages(firstUser.Id, 5));
            Insert("UserStoryImage", FakeDataFactory.GenerateStoryImages(lastUser.Id, 5));

            var images = GetFromDatabase<UserStoryImage>("SELECT * FROM UserStoryImage");

            var response = await _httpClient.GetAsync(Endpoint);
            var content = await response.Content.ReadFromJsonAsync<IEnumerable<UserStoryModel>>();

            Assert.Equal(users.Count(), content.Count());
            Assert.Equivalent(content.Select(x => x.UserId), users.Select(x => x.Id));
            Assert.All(content.Where(x => x.UserId == firstUser.Id), story =>
            {
                Assert.Equal(firstUser.Nickname, story.UserName);
                Assert.Equivalent(images.Where(x => x.UserId == firstUser.Id).Select(x => x.Id), story.Images.Select(x => x.Id));
            });
            Assert.All(content.Where(x => x.UserId == lastUser.Id), story =>
            {
                Assert.Equal(lastUser.Nickname, story.UserName);
                Assert.Equivalent(images.Where(x => x.UserId == lastUser.Id).Select(x => x.Id), story.Images.Select(x => x.Id));
            });
        }
    }
}
