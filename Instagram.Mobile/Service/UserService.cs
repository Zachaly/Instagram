using Instagram.Models.User.Request;
using System.Net.Http.Json;

namespace Instagram.Mobile.Service
{
    public interface IUserService
    {
        Task<bool> RegisterAsync(RegisterRequest registerRequest);
    }

    public class UserService : IUserService
    {
        private readonly HttpClient _httpClient;

        private const string Endpoint = "user";

        public UserService(IHttpClientFactory httpClientFactory)
        {
            _httpClient = httpClientFactory.Create();
        }

        public async Task<bool> RegisterAsync(RegisterRequest registerRequest)
        {
            var response = await _httpClient.PostAsJsonAsync($"{Endpoint}", registerRequest);

            if(response.IsSuccessStatusCode)
            {
                return true;
            }
            return false;
        }
    }
}
