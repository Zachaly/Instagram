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
            var userQuery = new SqlQueryBuilder().BuildInsert("User", user).Build();
            ExecuteQuery(userQuery, user);

            var userId = GetFromDatabase<long>("SELECT Id FROM [User]").First();

            foreach(var post in FakeDataFactory.GeneratePosts(5, userId))
            {
                var query = new SqlQueryBuilder().BuildInsert("Post", post).Build();
                ExecuteQuery(query, post);
            }

            var postIds = GetFromDatabase<long>("SELECT Id FROM [Post]");

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

            foreach (var p in FakeDataFactory.GeneratePosts(5, userId))
            {
                var query = new SqlQueryBuilder().BuildInsert("Post", p).Build();
                ExecuteQuery(query, p);
            }

            var post = GetFromDatabase<Post>("SELECT * FROM [Post]").First();

            var response = await _httpClient.GetAsync($"{Endpoint}/{post.Id}");
            var content = await response.Content.ReadFromJsonAsync<PostModel>();

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.Equal(post.Id, content.Id);
            Assert.Equal(post.Content, content.Content);
            Assert.Equal(post.Created, content.Created);
            Assert.Equal(post.CreatorId, content.CreatorId);
            Assert.Equal(user.Nickname, content.CreatorName);
        }

        [Fact]
        public async Task GetByIdAsync_PostNotFound_ReturnsNotFound()
        {
            var response = await _httpClient.GetAsync($"{Endpoint}/2137");

            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        }
    }
}
