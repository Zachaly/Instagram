using Instagram.Models.Post;
using Instagram.Models.Post.Request;
using System.Net;
using System.Net.Http.Json;

namespace Instagram.Mobile.Service
{
    public interface IPostService
    {
        Task<int> GetCountAsync(GetPostRequest request);
        Task<IEnumerable<PostModel>> GetAsync(GetPostRequest request);
        Task<PostModel> GetByIdAsync(long id);
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
            var response = await _httpClient.GetAsync(request.BuildQuery(Endpoint));

            return await response.Content.ReadFromJsonAsync<IEnumerable<PostModel>>();
        }

        public async Task<PostModel> GetByIdAsync(long id)
        {
            var response = await _httpClient.GetAsync($"{Endpoint}/{id}");

            if(response.StatusCode == HttpStatusCode.NotFound)
            {
                throw new NotFoundException("Post");
            }

            return await response.Content.ReadFromJsonAsync<PostModel>();
        }
    }
}
