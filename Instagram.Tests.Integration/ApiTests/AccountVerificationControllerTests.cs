using Instagram.Domain.Entity;
using Instagram.Models.AccountVerification;
using Instagram.Tests.Integration.ApiTests.Infrastructure;
using System.Collections.Immutable;
using System.Net;
using System.Net.Http.Json;

namespace Instagram.Tests.Integration.ApiTests
{
    public class AccountVerificationControllerTests : ApiTest
    {
        const string Endpoint = "/api/account-verification";

        [Fact]
        public async Task GetAsync_ReturnsVerificationRequests()
        {
            await AuthorizeModerator();

            Insert("User", FakeDataFactory.GenerateUsers(2));

            var userIds = GetFromDatabase<long>("SELECT Id FROM [User]").Skip(1);

            var verificationsToInsert = userIds.SelectMany(id => FakeDataFactory.GenerateAccountVerification(id, 3));

            Insert("AccountVerification", verificationsToInsert);

            var verificationIds = GetFromDatabase<long>("SELECT Id FROM AccountVerification");

            var response = await _httpClient.GetAsync(Endpoint);
            var content = await response.Content.ReadFromJsonAsync<IEnumerable<AccountVerificationModel>>();

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.Equivalent(verificationIds, content.Select(x => x.Id));
        }

        [Fact]
        public async Task GetByIdAsync_ReturnSpecifiedRequest()
        {
            await AuthorizeModerator();

            Insert("User", FakeDataFactory.GenerateUsers(2));

            var userIds = GetFromDatabase<long>("SELECT Id FROM [User]").Skip(1);

            var verificationsToInsert = userIds.SelectMany(id => FakeDataFactory.GenerateAccountVerification(id, 3));

            Insert("AccountVerification", verificationsToInsert);

            var verificationId = GetFromDatabase<long>("SELECT Id FROM AccountVerification").First();

            var verification = GetFromDatabase<AccountVerification>("SELECT * FROM AccountVerification WHERE Id=@Id", new { Id = verificationId })
                .First();

            var response = await _httpClient.GetAsync($"{Endpoint}/{verificationId}");
            var content = await response.Content.ReadFromJsonAsync<AccountVerificationModel>();

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.Equal(verification.Id, content.Id);
            Assert.Equal(verification.LastName, content.LastName);
            Assert.Equal(verification.DateOfBirth, content.DateOfBirth);
            Assert.Equal(verification.FirstName, content.FirstName);
        }

        [Fact]
        public async Task GetByIdAsync_NonExistentId_NotFound()
        {
            await AuthorizeModerator();

            var response = await _httpClient.GetAsync($"{Endpoint}/2137");

            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        }

        [Fact]
        public async Task GetCountAsync_ReturnsProperCount()
        {
            await AuthorizeModerator();

            const int Count = 20;

            Insert("AccountVerification", FakeDataFactory.GenerateAccountVerification(1, Count));

            var response = await _httpClient.GetAsync($"{Endpoint}/count");
            var content = await response.Content.ReadFromJsonAsync<int>();

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.Equal(Count, content);
        }
    }
}
