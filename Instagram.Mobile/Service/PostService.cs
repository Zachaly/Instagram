using Instagram.Models.Post.Request;
using System.Net.Http.Json;

namespace Instagram.Mobile.Service
{
    public interface IPostService
    {
        Task<int> GetCountAsync(GetPostRequest request);
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
    }
}
