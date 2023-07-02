using Instagram.Tests.Integration.ApiTests.Infrastructure;
using System.Net;

namespace Instagram.Tests.Integration.ApiTests
{
    public class AuthPolicyTests : ApiTest
    {
        const string AdminEndpoint = "/api/test/admin";
        const string ModeratorEndpoint = "/api/test/moderator";
        const string NotBannedEndpoint = "/api/test/not-banned";

        [Fact]
        public async Task AdminPolicy_AdminAllowed()
        {
            await AuthorizeAdmin();

            var response = await _httpClient.GetAsync(AdminEndpoint);

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }

        [Fact]
        public async Task AdminPolicy_ModeratorNotAllowed()
        {
            await AuthorizeModerator();

            var response = await _httpClient.GetAsync(AdminEndpoint);

            Assert.Equal(HttpStatusCode.Forbidden, response.StatusCode);
        }

        [Fact]
        public async Task AdminPolicy_BannedNotAllowed()
        {
            await AuthorizeBanned();

            var response = await _httpClient.GetAsync(AdminEndpoint);

            Assert.Equal(HttpStatusCode.Forbidden, response.StatusCode);
        }

        [Fact]
        public async Task ModeratorPolicy_AdminAllowed()
        {
            await AuthorizeAdmin();

            var response = await _httpClient.GetAsync(ModeratorEndpoint);

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }

        [Fact]
        public async Task ModeratorPolicy_ModeratorAllowed()
        {
            await AuthorizeModerator();

            var response = await _httpClient.GetAsync(ModeratorEndpoint);

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }

        [Fact]
        public async Task ModeratorPolicy_BannedNotAllowed()
        {
            await AuthorizeBanned();

            var response = await _httpClient.GetAsync(ModeratorEndpoint);

            Assert.Equal(HttpStatusCode.Forbidden, response.StatusCode);
        }

        [Fact]
        public async Task NotBannedPolicy_AdminAllowed()
        {
            await AuthorizeAdmin();

            var response = await _httpClient.GetAsync(NotBannedEndpoint);

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }

        [Fact]
        public async Task NotBannedPolicy_ModeratorAllowed()
        {
            await AuthorizeModerator();

            var response = await _httpClient.GetAsync(NotBannedEndpoint);

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }

        [Fact]
        public async Task NotBannedPolicy_NotBannedUserAllowed()
        {
            await Authorize();

            var response = await _httpClient.GetAsync(NotBannedEndpoint);

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }

        [Fact]
        public async Task NotBannedPolicy_BannedUserNotAllowed()
        {
            await AuthorizeBanned();

            var response = await _httpClient.GetAsync(NotBannedEndpoint);

            Assert.Equal(HttpStatusCode.Forbidden, response.StatusCode);
        }
    }
}
