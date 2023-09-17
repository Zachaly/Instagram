using Instagram.Mobile.Extension;
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
        Task UpdateAsync(UpdateUserRequest request);
        Task UpdateProfilePictureAsync(long userId, string filePath);
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

        public Task<UserModel> GetByIdAsync(long id)
            => _httpClient.GetByIdAsync<UserModel>(Endpoint, id);

        public Task<IEnumerable<UserModel>> GetAsync(GetUserRequest request)
            => _httpClient.GetWithRequestAsync<UserModel, GetUserRequest>(Endpoint, request);

        public async Task UpdateAsync(UpdateUserRequest request)
        {
            var response = await _httpClient.PatchAsJsonAsync(Endpoint, request);

            if(response.StatusCode == HttpStatusCode.BadRequest)
            {
                throw new InvalidRequestException(JsonConvert.DeserializeObject<ResponseModel>(await response.Content.ReadAsStringAsync()));
            }
        }

        public async Task UpdateProfilePictureAsync(long userId, string filePath)
        {
            using var request = new MultipartFormDataContent
            {
                { new StringContent(userId.ToString()), "UserId" }
            };

            if(filePath is not null)
            {
                request.AddFileContent(filePath, "File");
            }
            
            await _httpClient.PatchAsync($"{Configuration.ApiUrl}image/profile", request);
        }
    }
}
