using Dapper;
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
        private readonly string[] _truncateQueries =
        {
            "TRUNCATE TABLE [User]"
        };


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

            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue($"Bearer", content.AuthToken);
        }

        public void Dispose()
        {
            using (var connection = new SqlConnection(Constants.ConnectionString))
            {
                foreach (var query in _truncateQueries)
                {
                    connection.Query(query);
                }
            }
        }
    }
}
