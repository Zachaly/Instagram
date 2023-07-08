using Instagram.Api.Authorization;
using Instagram.Domain.Entity;
using Instagram.Models.User;
using Instagram.Models.User.Request;
using Microsoft.Extensions.Configuration;
using System.Net.Http.Headers;
using System.Net.Http.Json;

[assembly: CollectionBehavior(DisableTestParallelization = true)]
namespace Instagram.Tests.Integration.ApiTests.Infrastructure
{
    public class ApiTest : DatabaseTest
    {
        protected readonly HttpClient _httpClient;
        protected long _authorizedUserId = 0;

        public ApiTest()
        {
            var webFactory = new InstagramApplicationFactory()
            .WithWebHostBuilder(builder =>
            {
                builder.ConfigureAppConfiguration((context, config) =>
                {
                    config.AddInMemoryCollection(new Dictionary<string, string?>
                    {
                        ["ConnectionStrings:SqlConnection"] = Constants.ConnectionString,
                        ["DatabaseName"] = Constants.Database
                    });
                });
            });

            _httpClient = webFactory.CreateClient();
        }

        protected async Task Authorize()
        {
            var registerRequest = new RegisterRequest
            {
                Email = "email@email.com",
                Gender = 0,
                Name = "username",
                Nickname = "nickname",
                Password = "password",
            };
            await _httpClient.PostAsJsonAsync("/api/user", registerRequest);

            var loginRequest = new LoginRequest
            {
                Email = registerRequest.Email,
                Password = registerRequest.Password,
            };

            var response = await _httpClient.PostAsJsonAsync("/api/user/login", loginRequest);
            var content = await response.Content.ReadFromJsonAsync<LoginResponse>();

            _authorizedUserId = content.UserId;
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue($"Bearer", content.AuthToken);
        }
        
        protected async Task AuthorizeModerator()
        {
            var registerRequest = new RegisterRequest
            {
                Email = "email@email.com",
                Gender = 0,
                Name = "moderator",
                Nickname = "nickname",
                Password = "password",
            };
            await _httpClient.PostAsJsonAsync("/api/user", registerRequest);

            var moderatorId = GetFromDatabase<long>("SELECT Id FROM [User] WHERE Name=@Name", new { Name = registerRequest.Name }).First();

            Insert("UserClaim", new UserClaim { UserId = moderatorId, Value = UserClaimValues.Moderator });

            var loginRequest = new LoginRequest
            {
                Email = registerRequest.Email,
                Password = registerRequest.Password,
            };

            var response = await _httpClient.PostAsJsonAsync("/api/user/login", loginRequest);
            var content = await response.Content.ReadFromJsonAsync<LoginResponse>();

            _authorizedUserId = content.UserId;
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue($"Bearer", content.AuthToken);
        }

        protected async Task AuthorizeAdmin()
        {
            var loginRequest = new LoginRequest
            {
                Email = "admin@instagram.com",
                Password = "zaq1@WSX"
            };

            var response = await _httpClient.PostAsJsonAsync("/api/user/login", loginRequest);
            var content = await response.Content.ReadFromJsonAsync<LoginResponse>();

            _authorizedUserId = content.UserId;
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue($"Bearer", content.AuthToken);
        }

        protected async Task AuthorizeBanned()
        {
            var registerRequest = new RegisterRequest
            {
                Email = "email@email.com",
                Gender = 0,
                Name = "banned",
                Nickname = "nickname",
                Password = "password",
            };
            await _httpClient.PostAsJsonAsync("/api/user", registerRequest);

            var userId = GetFromDatabase<long>("SELECT Id FROM [User] WHERE Name=@Name", new { Name = registerRequest.Name }).First();

            Insert("UserClaim", new UserClaim { UserId = userId, Value = UserClaimValues.Ban });

            var loginRequest = new LoginRequest
            {
                Email = registerRequest.Email,
                Password = registerRequest.Password,
            };

            var response = await _httpClient.PostAsJsonAsync("/api/user/login", loginRequest);
            var content = await response.Content.ReadFromJsonAsync<LoginResponse>();

            _authorizedUserId = content.UserId;
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue($"Bearer", content.AuthToken);
        }
    }
}
