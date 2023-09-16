using Instagram.Mobile.Extension;
using Instagram.Models.Post;
using Instagram.Models.Post.Request;
using Instagram.Models.Response;
using Newtonsoft.Json;
using System.Net;

namespace Instagram.Mobile.Service
{
    public interface IPostService
    {
        Task<int> GetCountAsync(GetPostRequest request);
        Task<IEnumerable<PostModel>> GetAsync(GetPostRequest request);
        Task<PostModel> GetByIdAsync(long id);
        Task AddAsync(long userId, string content, IEnumerable<string> images, IEnumerable<string> tags);
    }

    public class PostService : IPostService
    {
        private readonly HttpClient _httpClient;
        private const string Endpoint = "post";

        public PostService(IHttpClientFactory httpClientFactory)
        {
            _httpClient = httpClientFactory.Create();
        }

        public Task<int> GetCountAsync(GetPostRequest request)
            => _httpClient.GetCountAsync(Endpoint, request);

        public Task<IEnumerable<PostModel>> GetAsync(GetPostRequest request)
            => _httpClient.GetWithRequestAsync<PostModel, GetPostRequest>(Endpoint, request);

        public Task<PostModel> GetByIdAsync(long id)
            => _httpClient.GetByIdAsync<PostModel>(Endpoint, id);

        public async Task AddAsync(long userId, string content, IEnumerable<string> images, IEnumerable<string> tags)
        {
            using var request = new MultipartFormDataContent()
            {
                { new StringContent(userId.ToString()), "CreatorId" },
                { new StringContent(content), "Content" },
            };

            request.AddFileContent(images, "Files");

            foreach(var tag in tags)
            {
                request.Add(new StringContent(tag), "Tags");
            }

            var response = await _httpClient.PostAsync(Endpoint, request);

            if(response.StatusCode == HttpStatusCode.BadRequest)
            {
                var responseContent = await response.Content.ReadAsStringAsync();
                throw new InvalidRequestException(JsonConvert.DeserializeObject<ResponseModel>(responseContent));
            }
        }
    }
}
