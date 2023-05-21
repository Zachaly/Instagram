using Dapper;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;

[assembly: CollectionBehavior(DisableTestParallelization = true)]
namespace Instagram.Tests.Integration.ApiTests
{
    public class ApiTest : IDisposable
    {
        protected readonly HttpClient _httpClient;
        protected const string ConnectionString = "server=.; database=InstagramTest; Integrated Security=true";
        protected const string MasterConnection = "server=.; database=master; Integrated Security=true";
        protected const string DatabaseName = "InstagramTest";

        public ApiTest()
        {
            var webFactory = new WebApplicationFactory<Program>()
            .WithWebHostBuilder(builder =>
            {
                builder.ConfigureAppConfiguration((context, config) =>
                {
                    config.AddInMemoryCollection(new Dictionary<string, string?>
                    {
                        ["ConnectionStrings:DatabaseConnection"] = ConnectionString,
                        ["DatabaseName"] = DatabaseName
                    });
                });
            });

            _httpClient = webFactory.CreateClient();
        }

        public void Dispose()
        {
            using(var connection = new SqlConnection(MasterConnection))
            {
                connection.Query($"DROP DATABASE {DatabaseName}");
            }
        }
    }
}
