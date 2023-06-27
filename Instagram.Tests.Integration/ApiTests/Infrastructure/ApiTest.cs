﻿using Dapper;
using Instagram.Api.Authorization;
using Instagram.Database.Sql;
using Instagram.Domain.Entity;
using Instagram.Models.User;
using Instagram.Models.User.Request;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System.Net.Http.Headers;
using System.Net.Http.Json;

[assembly: CollectionBehavior(DisableTestParallelization = true)]
namespace Instagram.Tests.Integration.ApiTests.Infrastructure
{
    public class ApiTest : IDisposable
    {
        protected readonly HttpClient _httpClient;
        protected const string DatabaseName = "InstagramTest";
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
                        ["DatabaseName"] = DatabaseName
                    });
                });
            });

            _httpClient = webFactory.CreateClient();
        }

        protected IEnumerable<T> GetFromDatabase<T>(string query, object param = null)
        {
            using(var connection = new SqlConnection(Constants.ConnectionString))
            {
                return connection.Query<T>(query, param);
            }
        }

        protected void ExecuteQuery(string query, object param = null)
        {
            using (var connection = new SqlConnection(Constants.ConnectionString))
            {
                connection.Query(query, param);
            }
        }

        protected void Insert<T>(string table, T item) where T : IEntity
        {
            var query = new SqlQueryBuilder().BuildInsert(table, item).Build();

            ExecuteQuery(query, item);
        }

        protected void Insert<T>(string table, IEnumerable<T> items) where T : IEntity
        {
            foreach (var item in items)
            {
                var query = new SqlQueryBuilder().BuildInsert(table, item).Build();
                ExecuteQuery(query, item);
            }
        }

        protected async Task Authorize()
        {
            var registerRequest = new RegisterRequest
            {
                Email = "email@email.com",
                Gender = 0,
                Name = "name",
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

        public void Dispose()
        {
            using (var connection = new SqlConnection(Constants.ConnectionString))
            {
                foreach (var query in Constants.TruncateQueries)
                {
                    connection.Query(query);
                }
            }
        }
    }
}
