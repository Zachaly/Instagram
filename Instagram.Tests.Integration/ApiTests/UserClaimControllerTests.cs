using Instagram.Domain.Entity;
using Instagram.Models.UserClaim;
using Instagram.Models.UserClaim.Request;
using Instagram.Tests.Integration.ApiTests.Infrastructure;
using System.Net;
using System.Net.Http.Json;

namespace Instagram.Tests.Integration.ApiTests
{
    public class UserClaimControllerTests : ApiTest
    {
        const string Endpoint = "/api/user-claim";

        [Fact]
        public async Task GetAsync_ReturnsClaims()
        {
            await AuthorizeAdmin();

            Insert("User", FakeDataFactory.GenerateUsers(2));

            var users = GetFromDatabase<User>("SELECT * FROM [User] WHERE Name!='admin'");

            var claims = FakeDataFactory.GenerateUserClaims(users.First().Id, 2);
            claims.AddRange(FakeDataFactory.GenerateUserClaims(users.Last().Id, 2));

            Insert("UserClaim", claims);

            var response = await _httpClient.GetAsync(Endpoint);
            var content = await response.Content.ReadFromJsonAsync<IEnumerable<UserClaimModel>>();

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.All(content.Where(x => x.UserId == users.Last().Id), c => Assert.Equal(users.Last().Nickname, c.UserName));
            Assert.All(content.Where(x => x.UserId == users.First().Id), c => Assert.Equal(users.First().Nickname, c.UserName));
            Assert.Equivalent(claims.Select(x => x.Value), content.Select(x => x.Value));
        }

        [Fact]
        public async Task GetCountAsync_ReturnsCount()
        {
            await AuthorizeAdmin();

            const int Count = 10;

            Insert("UserClaim", FakeDataFactory.GenerateUserClaims(2, Count));

            var response = await _httpClient.GetAsync($"{Endpoint}/count");
            var content = await response.Content.ReadFromJsonAsync<int>();

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.Equal(Count + 1, content);
        }

        [Fact]
        public async Task PostAsync_AddsClaim()
        {
            await AuthorizeAdmin();

            var request = new AddUserClaimRequest
            {
                UserId = 21,
                Value = "val"
            };

            var response = await _httpClient.PostAsJsonAsync(Endpoint, request);

            Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);
            Assert.Contains(GetFromDatabase<UserClaim>("SELECT * FROM UserClaim"), x => x.UserId == request.UserId && x.Value == request.Value);
        }

        [Fact]
        public async Task PostAsync_InvalidRequest_Failure()
        {
            await AuthorizeAdmin();

            var request = new AddUserClaimRequest
            {
                UserId = 21,
                Value = ""
            };

            var response = await _httpClient.PostAsJsonAsync(Endpoint, request);
            var content = await ReadErrorResponse(response);

            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
            Assert.Contains(content.ValidationErrors.Keys, x => x == "Value");
            Assert.DoesNotContain(GetFromDatabase<UserClaim>("SELECT * FROM UserClaim"), x => x.UserId == request.UserId && x.Value == request.Value);
        }

        [Fact]
        public async Task DeleteAsync_DeletesSpecifiedClaim()
        {
            await AuthorizeAdmin();

            var claim = FakeDataFactory.GenerateUserClaims(2, 1).First();

            Insert("UserClaim", claim);

            var response = await _httpClient.DeleteAsync($"{Endpoint}/{claim.UserId}/{claim.Value}");

            var updatedClaims = GetFromDatabase<UserClaim>("SELECT * FROM UserClaim");

            Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);
            Assert.DoesNotContain(updatedClaims, x => x.UserId == claim.UserId && x.Value == claim.Value);
        }
    }
}
