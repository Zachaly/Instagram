using Instagram.Models.Response;
using Instagram.Models.User.Request;
using Newtonsoft.Json;
using System.Net.Http.Json;

namespace Instagram.Mobile.Service
{
    public interface IUserService
    {
        Task RegisterAsync(RegisterRequest registerRequest);
    }

    public class UserService : IUserService
    {
        private readonly HttpClient _httpClient;

        private const string Endpoint = "user";

        public UserService(IHttpClientFactory httpClientFactory)
        {
            _httpClient = httpClientFactory.Create();
        }

        public async Task RegisterAsync(RegisterRequest registerRequest)
        {
            var response = await _httpClient.PostAsJsonAsync($"{Endpoint}", registerRequest);

            if(response.IsSuccessStatusCode)
            {
                return;
            }
            
            var content = await response.Content.ReadAsStringAsync();

            throw new InvalidRequestException(JsonConvert.DeserializeObject<ResponseModel>(content));
        }
    }
}
