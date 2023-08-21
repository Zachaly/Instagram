using Instagram.Models.User;
using Instagram.Models.User.Request;
using System.Net;
using System.Net.Http.Json;
using Newtonsoft.Json;
using Instagram.Models.Response;

namespace Instagram.Mobile.Service
{
    public interface IAuthorizationService
    {
        LoginResponse UserData { get; }
        bool IsAuthorized { get; }
        Task AuthorizeAsync(LoginRequest request);
        Task LogoutAsync();
    }

    public class AuthorizationService : IAuthorizationService
    {
        private LoginResponse _userData = null;
        private readonly IHttpClientFactory _httpClientFactory;

        public LoginResponse UserData => _userData;

        public bool IsAuthorized => _userData is not null;

        public AuthorizationService(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        public async Task AuthorizeAsync(LoginRequest request)
        {
            var client = _httpClientFactory.Create();

            var response = await client.PostAsJsonAsync("user/login", request);

            if (response.IsSuccessStatusCode)
            {
                var userData = await response.Content.ReadFromJsonAsync<LoginResponse>();

                _userData = userData;
                _httpClientFactory.AddAuthToken(userData.AuthToken);
            } 
            else if(response.StatusCode == HttpStatusCode.BadRequest)
            {
                var content = await response.Content.ReadAsStringAsync();
                var responseModel = JsonConvert.DeserializeObject<DataResponseModel<LoginResponse>>(content);

                throw new InvalidRequestException(responseModel);
            }
        }

        public Task LogoutAsync()
        {
            _userData = null;

            _httpClientFactory.AddAuthToken("");

            return Task.CompletedTask;
        }
    }
}
