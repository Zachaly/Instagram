using Instagram.Models.User;
using Instagram.Models.User.Request;
using System.Diagnostics;
using System.Net.Http.Json;

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
            try
            {
                var client = _httpClientFactory.Create();

                var response = await client.PostAsJsonAsync("user/login", request);

                if (response.IsSuccessStatusCode)
                {
                    var userData = await response.Content.ReadFromJsonAsync<LoginResponse>();

                    _userData = userData;
                    _httpClientFactory.AddAuthToken(userData.AuthToken);
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
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
