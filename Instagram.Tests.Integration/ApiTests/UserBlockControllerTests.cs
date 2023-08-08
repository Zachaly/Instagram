using Instagram.Domain.Entity;
using Instagram.Models.UserBlock;
using Instagram.Models.UserBlock.Request;
using Instagram.Tests.Integration.ApiTests.Infrastructure;
using System.Net;
using System.Net.Http.Json;

namespace Instagram.Tests.Integration.ApiTests
{
    public class UserBlockControllerTests : ApiTest
    {
        const string Endpoint = "/api/user-block";

        [Fact]
        public async Task GetAsync_ReturnsBlocks()
        {
            await Authorize();

            Insert("User", FakeDataFactory.GenerateUsers(4));

            var users = GetFromDatabase<User>("SELECT * FROM [User]");

            Insert("UserBlock", FakeDataFactory.GenerateUserBlocks(users.First().Id, users.Skip(1).Select(x => x.Id)));

            var blockIds = GetFromDatabase<long>("SELECT Id FROM UserBlock");

            var response = await _httpClient.GetAsync(Endpoint);
            var content = await response.Content.ReadFromJsonAsync<IEnumerable<UserBlockModel>>();

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.Equivalent(blockIds, content.Select(x => x.Id));
            Assert.All(content, block =>
            {
                Assert.Equal(users.First(u => u.Id == block.UserId).Nickname, block.UserName);
            });
        }

        [Fact]
        public async Task GetByIdAsync_ReturnsSpecifiedBlock()
        {
            await Authorize();

            Insert("User", FakeDataFactory.GenerateUsers(4));

            var users = GetFromDatabase<User>("SELECT * FROM [User]");

            Insert("UserBlock", FakeDataFactory.GenerateUserBlocks(users.First().Id, users.Skip(1).Select(x => x.Id)));

            var block = GetFromDatabase<UserBlock>("SELECT * FROM UserBlock").First();
            var userName = GetFromDatabase<string>("SELECT Nickname FROM [User] WHERE Id=@Id", new { Id = block.BlockedUserId }).First();

            var response = await _httpClient.GetAsync($"{Endpoint}/{block.Id}");
            var content = await response.Content.ReadFromJsonAsync<UserBlockModel>();

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.Equal(block.BlockedUserId, content.UserId);
            Assert.Equal(userName, content.UserName);
        }

        [Fact]
        public async Task GetByIdAsync_Failure_UserBlockNotFound()
        {
            await Authorize();

            var response = await _httpClient.GetAsync($"{Endpoint}/2137");

            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        }

        [Fact]
        public async Task GetCountAsync_ReturnsProperCount()
        {
            await Authorize();

            var userIds = new long[] { 1, 2, 3, 4, 5, 6, 7, 8, 9 };

            Insert("UserBlock", FakeDataFactory.GenerateUserBlocks(2137, userIds));

            var response = await _httpClient.GetAsync($"{Endpoint}/count");
            var content = await response.Content.ReadFromJsonAsync<int>();

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.Equal(content, userIds.Length);
        }

        [Fact]
        public async Task PostAsync_AddsUserBlock()
        {
            await Authorize();

            var request = new AddUserBlockRequest
            {
                BlockedUserId = 1,
                BlockingUserId = 2
            };

            var response = await _httpClient.PostAsJsonAsync(Endpoint, request);

            var blocks = GetFromDatabase<UserBlock>("SELECT * FROM UserBlock");

            Assert.Equal(HttpStatusCode.Created, response.StatusCode);
            Assert.Contains(blocks, x => x.BlockedUserId == request.BlockedUserId && x.BlockingUserId == request.BlockingUserId);
        }

        [Fact]
        public async Task PostAsync_InvalidBlockedUserId_BadRequest()
        {
            await Authorize();

            var request = new AddUserBlockRequest
            {
                BlockedUserId = 0,
                BlockingUserId = 2
            };

            var response = await _httpClient.PostAsJsonAsync(Endpoint, request);
            var content = await ReadErrorResponse(response);

            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
            Assert.Contains(content.ValidationErrors!.Keys, x => x == "BlockedUserId");
        }

        [Fact]
        public async Task DeleteByIdAsync_DeletesCorrectUserBlock()
        {
            await Authorize();

            var userIds = new long[] { 1, 2, 3, 4, 5, 6, 7, 8, 9 };

            Insert("UserBlock", FakeDataFactory.GenerateUserBlocks(2137, userIds));

            var block = GetFromDatabase<UserBlock>("SELECT * FROM UserBlock").First();

            var response = await _httpClient.DeleteAsync($"{Endpoint}/{block.Id}");

            var blocks = GetFromDatabase<UserBlock>("SELECT * FROM UserBlock");

            Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);
            Assert.DoesNotContain(blocks, x => x.Id == block.Id);
        }
    }
}
