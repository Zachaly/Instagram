using Instagram.Mobile.Extension;
using Instagram.Models.PostLike;
using Instagram.Models.PostLike.Request;
using System.Net.Http.Json;

namespace Instagram.Mobile.Service
{
    public interface IPostLikeService
    {
        Task<int> GetCountAsync(GetPostLikeRequest request);
        Task<IEnumerable<PostLikeModel>> GetAsync(GetPostLikeRequest request);
        Task AddAsync(AddPostLikeRequest request);
        Task DeleteAsync(long userId, long postId);
    }

    public class PostLikeService : IPostLikeService
    {
        private readonly HttpClient _httpClient;
        private const string Endpoint = "post-like";

        public PostLikeService(IHttpClientFactory httpClientFactory)
        {
            _httpClient = httpClientFactory.Create();
        }

        public async Task AddAsync(AddPostLikeRequest request)
        {
            await _httpClient.PostAsJsonAsync(Endpoint, request);
        }

        public async Task<IEnumerable<PostLikeModel>> GetAsync(GetPostLikeRequest request)
            => await _httpClient.GetWithRequestAsync<PostLikeModel, GetPostLikeRequest>(Endpoint, request);

        public Task<int> GetCountAsync(GetPostLikeRequest request)
            => _httpClient.GetCountAsync(Endpoint, request);

        public async Task DeleteAsync(long userId, long postId)
        {
            await _httpClient.DeleteAsync($"{Endpoint}/{postId}/{userId}");
        }
    }
}
