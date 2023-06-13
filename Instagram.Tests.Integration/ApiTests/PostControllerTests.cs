using Instagram.Database.Sql;
using Instagram.Domain.Entity;
using Instagram.Models.Post;
using Instagram.Tests.Integration.ApiTests.Infrastructure;
using System.Net;
using System.Net.Http.Json;

namespace Instagram.Tests.Integration.ApiTests
{
    public class PostControllerTests : ApiTest
    {
        const string Endpoint = "/api/post";

        [Fact]
        public async Task GetAsync_ReturnsPosts()
        {
            var user = FakeDataFactory.GenerateUsers(1).First();
            Insert("User", user);

            var userId = GetFromDatabase<long>("SELECT Id FROM [User]").First();

            Insert("Post", FakeDataFactory.GeneratePosts(5, userId));

            var postIds = GetFromDatabase<long>("SELECT Id FROM [Post]");

            foreach (var postId in postIds)
            {
                Insert("PostImage", FakeDataFactory.GeneratePostImages(postId, 2));
            }

            var response = await _httpClient.GetAsync(Endpoint);
            var content = await response.Content.ReadFromJsonAsync<IEnumerable<PostModel>>();

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.Equivalent(postIds, content.Select(x => x.Id));
            Assert.All(content, post => Assert.Equal(user.Nickname, post.CreatorName));
        }

        [Fact]
        public async Task GetByIdAsync_ReturnsSpecifiedPost()
        {
            var user = FakeDataFactory.GenerateUsers(1).First();
            var userQuery = new SqlQueryBuilder().BuildInsert("User", user).Build();
            ExecuteQuery(userQuery, user);

            var userId = GetFromDatabase<long>("SELECT Id FROM [User]").First();

            Insert("Post", FakeDataFactory.GeneratePosts(5, userId));

            var post = GetFromDatabase<Post>("SELECT * FROM [Post]").First();

            Insert("PostImage", FakeDataFactory.GeneratePostImages(post.Id, 2));

            var imageIds = GetFromDatabase<long>("SELECT Id FROM PostImage");

            var response = await _httpClient.GetAsync($"{Endpoint}/{post.Id}");
            var content = await response.Content.ReadFromJsonAsync<PostModel>();

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.Equal(post.Id, content.Id);
            Assert.Equal(post.Content, content.Content);
            Assert.Equal(post.Created, content.Created);
            Assert.Equal(post.CreatorId, content.CreatorId);
            Assert.Equal(user.Nickname, content.CreatorName);
            Assert.Equivalent(imageIds, content.ImageIds);
        }

        [Fact]
        public async Task GetByIdAsync_PostNotFound_ReturnsNotFound()
        {
            var response = await _httpClient.GetAsync($"{Endpoint}/2137");

            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        }

        [Fact]
        public async Task GetCountAsync_ReturnsProperCount()
        {
            const int PostCount = 15;

            Insert("Post", FakeDataFactory.GeneratePosts(PostCount, 2137));

            var response = await _httpClient.GetAsync($"{Endpoint}/count");
            var content = await response.Content.ReadFromJsonAsync<int>();

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.Equal(PostCount, content);
        }
    }
}
