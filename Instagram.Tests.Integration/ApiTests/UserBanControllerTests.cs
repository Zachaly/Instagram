using Instagram.Domain.Entity;
using Instagram.Models.UserBan;
using Instagram.Models.UserBan.Request;
using Instagram.Tests.Integration.ApiTests.Infrastructure;
using System.Net;
using System.Net.Http.Json;

namespace Instagram.Tests.Integration.ApiTests
{
    public class UserBanControllerTests : ApiTest
    {
        const string Endpoint = "/api/user-ban";

        [Fact]
        public async Task GetAsync_ReturnsBans()
        {
            await Authorize();

            var users = FakeDataFactory.GenerateUsers(5);

            Insert("User", users);

            var userIds = GetFromDatabase<long>("SELECT Id FROM [User] WHERE Nickname IN @Names", new { Names = users.Select(x => x.Nickname) });

            Insert("UserBan", FakeDataFactory.GenerateUserBans(userIds));

            var bans = GetFromDatabase<UserBan>("SELECT * FROM [UserBan]");

            var response = await _httpClient.GetAsync(Endpoint);
            var content = await response.Content.ReadFromJsonAsync<IEnumerable<UserBanModel>>();

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.Equivalent(bans.Select(x => x.Id), content.Select(x => x.Id));
        }

        [Fact]
        public async Task GetById_ReturnsSpecifiedBan()
        {
            await Authorize();

            var user = FakeDataFactory.GenerateUsers(1).First();

            Insert("User", user);

            var userId = GetFromDatabase<long>("SELECT Id FROM [User] WHERE Nickname=@Name", new { Name = user.Nickname }).First();

            Insert("UserBan", FakeDataFactory.GenerateUserBans(new long[] { userId }));

            var ban = GetFromDatabase<UserBan>("SELECT * FROM [UserBan]").First();

            var response = await _httpClient.GetAsync($"{Endpoint}/{ban.Id}");
            var content = await response.Content.ReadFromJsonAsync<UserBanModel>();

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.Equal(ban.Id, content.Id);
            Assert.Equal(ban.UserId, content.UserId);
            Assert.Equal(ban.StartDate, content.StartDate);
            Assert.Equal(ban.EndDate, content.EndDate);
            Assert.Equal(user.Nickname, content.UserName);
        }

        [Fact]
        public async Task GetById_BanNotFound_Failure()
        {
            await Authorize();

            var response = await _httpClient.GetAsync($"{Endpoint}/2137");

            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        }

        [Fact]
        public async Task GetCount_ReturnsProperCount()
        {
            await Authorize();

            Insert("User", FakeDataFactory.GenerateUsers(10));

            var userIds = GetFromDatabase<long>("SELECT Id FROM [User]");

            Insert("UserBan", FakeDataFactory.GenerateUserBans(userIds));

            var response = await _httpClient.GetAsync($"{Endpoint}/count");
            var content = await response.Content.ReadFromJsonAsync<int>();

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.Equal(userIds.Count(), content);
        }

        [Fact]
        public async Task AddAsync_AddsNewBan()
        {
            await AuthorizeModerator();

            var request = new AddUserBanRequest 
            { 
                EndDate = DateTimeOffset.UtcNow.AddDays(1).ToUnixTimeMilliseconds(),
                UserId = 2 
            };

            var response = await _httpClient.PostAsJsonAsync(Endpoint, request);

            var bans = GetFromDatabase<UserBan>("SELECT * FROM UserBan");

            Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);
            Assert.Contains(bans, x => x.UserId == request.UserId && x.EndDate == request.EndDate);
        }

        [Fact]
        public async Task AddAsync_InvaliRequest_Failure()
        {
            await AuthorizeModerator();

            var request = new AddUserBanRequest
            {
                EndDate = 2137,
                UserId = 21
            };

            var response = await _httpClient.PostAsJsonAsync(Endpoint, request);
            var content = await ReadErrorResponse(response);

            var bans = GetFromDatabase<UserBan>("SELECT * FROM UserBan");

            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
            Assert.Contains(content.ValidationErrors.Keys, x => x == "EndDate");
            Assert.DoesNotContain(bans, x => x.UserId == request.UserId && x.EndDate == request.EndDate);
        }

        [Fact]
        public async Task DeleteAsync_RemovesCorrectBan()
        {
            await AuthorizeAdmin();

            Insert("UserBan", FakeDataFactory.GenerateUserBans(new long[] { 1, 2, 3, 4 }));

            var id = GetFromDatabase<long>("SELECT Id FROM UserBan").First();

            var response = await _httpClient.DeleteAsync($"{Endpoint}/{id}");

            var bans = GetFromDatabase<UserBan>("SELECT * FROM UserBan");

            Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);
            Assert.DoesNotContain(bans, x => x.Id == id);
        }
    }
}
