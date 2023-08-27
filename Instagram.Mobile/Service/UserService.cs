using Instagram.Models.Response;
using Instagram.Models.User;
using Instagram.Models.User.Request;
using Newtonsoft.Json;
using System.Net;
using System.Net.Http.Json;

namespace Instagram.Mobile.Service
{
    public interface IUserService
    {
        Task RegisterAsync(RegisterRequest registerRequest);
        Task<UserModel> GetByIdAsync(long id);
        Task<IEnumerable<UserModel>> GetAsync(GetUserRequest request);
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

        public async Task<UserModel> GetByIdAsync(long id)
        {
            var response = await _httpClient.GetAsync($"{Endpoint}/{id}");

            if(response.StatusCode == HttpStatusCode.NotFound)
            {
                throw new NotFoundException("User");
            }

            return await response.Content.ReadFromJsonAsync<UserModel>();
        }

        public async Task<IEnumerable<UserModel>> GetAsync(GetUserRequest request)
        {
            var response = await _httpClient.GetAsync(request.BuildQuery(Endpoint));

            return await response.Content.ReadFromJsonAsync<IEnumerable<UserModel>>();
        }
    }
}
