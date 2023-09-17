using Instagram.Models.User;
using Instagram.Models.User.Request;
using System.Net;
using System.Net.Http.Json;
using Newtonsoft.Json;
using Instagram.Models.Response;
using Instagram.Models.UserFollow.Request;
using Instagram.Models.UserFollow;
using Instagram.Mobile.Extension;

namespace Instagram.Mobile.Service
{
    public interface IAuthorizationService
    {
        LoginResponse UserData { get; }
        List<long> FollowedUserIds { get; }
        bool IsAuthorized { get; }
        Task AuthorizeAsync(LoginRequest request);
        Task LogoutAsync();
    }

    public class AuthorizationService : IAuthorizationService
    {
        private LoginResponse _userData = null;
        private readonly List<long> _followedUserIds = new List<long>();
        private readonly IHttpClientFactory _httpClientFactory;

        public LoginResponse UserData => _userData;

        public List<long> FollowedUserIds => _followedUserIds;

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

                var getFollowRequest = new GetUserFollowRequest
                {
                    SkipPagination = true,
                    FollowingUserId = _userData.UserId,
                };

                var followResponse = await client.GetAsync(getFollowRequest.BuildQuery("user-follow"));

                var follows = await followResponse.Content.ReadFromJsonAsync<IEnumerable<UserFollowModel>>();

                _followedUserIds.AddRange(follows.Select(f => f.FollowedUserId));
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
