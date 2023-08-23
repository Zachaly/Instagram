using Instagram.Models.Post;
using Instagram.Models.Post.Request;
using System.Net.Http.Json;

namespace Instagram.Mobile.Service
{
    public interface IPostService
    {
        Task<int> GetCountAsync(GetPostRequest request);
        Task<IEnumerable<PostModel>> GetAsync(GetPostRequest request);
    }

    public class PostService : IPostService
    {
        private readonly HttpClient _httpClient;
        private const string Endpoint = "post";

        public PostService(IHttpClientFactory httpClientFactory)
        {
            _httpClient = httpClientFactory.Create();
        }

        public async Task<int> GetCountAsync(GetPostRequest request)
        {
            var response = await _httpClient.GetAsync(request.BuildQuery($"{Endpoint}/count"));

            return await response.Content.ReadFromJsonAsync<int>();
        }

        public async Task<IEnumerable<PostModel>> GetAsync(GetPostRequest request)
        {
            var q = request.BuildQuery(Endpoint);
            var response = await _httpClient.GetAsync(q);

            return await response.Content.ReadFromJsonAsync<IEnumerable<PostModel>>();
        }
    }
}
