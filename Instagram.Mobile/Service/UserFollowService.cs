using Instagram.Mobile.Extension;
using Instagram.Models.UserFollow;
using Instagram.Models.UserFollow.Request;
using System.Net.Http.Json;

namespace Instagram.Mobile.Service
{
    public interface IUserFollowService
    {
        Task<int> GetCountAsync(GetUserFollowRequest request);
        Task AddFollowAsync(AddUserFollowRequest request);
        Task DeleteFollowAsync(long followerId, long followedUserId);
        Task<IEnumerable<UserFollowModel>> GetAsync(GetUserFollowRequest request);
    }

    public class UserFollowService : IUserFollowService
    {
        public const string Endpoint = "user-follow";
        private readonly HttpClient _httpClient;

        public UserFollowService(IHttpClientFactory httpClientFactory)
        {
            _httpClient = httpClientFactory.Create();
        }

        public async Task AddFollowAsync(AddUserFollowRequest request)
        {
            await _httpClient.PostAsJsonAsync(Endpoint, request);
        }

        public async Task DeleteFollowAsync(long followerId, long followedUserId)
        {
            await _httpClient.DeleteAsync($"{Endpoint}/{followerId}/{followedUserId}");
        }

        public Task<IEnumerable<UserFollowModel>> GetAsync(GetUserFollowRequest request)
            => _httpClient.GetWithRequestAsync<UserFollowModel, GetUserFollowRequest>(Endpoint, request);

        public Task<int> GetCountAsync(GetUserFollowRequest request)
            => _httpClient.GetCountAsync(Endpoint, request);
    }
}
