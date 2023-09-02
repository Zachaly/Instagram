using Instagram.Models.Post;
using Instagram.Models.Post.Request;
using Instagram.Models.Response;
using Newtonsoft.Json;
using System.Net;
using System.Net.Http.Headers;
using System.Net.Http.Json;

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

        public async Task AddAsync(long userId, string content, IEnumerable<string> images, IEnumerable<string> tags)
        {
            using var request = new MultipartFormDataContent()
            {
                { new StringContent(userId.ToString()), "CreatorId" },
                { new StringContent(content), "Content" },
            };

            foreach(var image in images)
            {
                var fileContent = new StreamContent(File.OpenRead(image));
                fileContent.Headers.ContentDisposition = new ContentDispositionHeaderValue("form-data")
                {
                    FileName = image,
                    Name = "Files"
                };
                fileContent.Headers.ContentType = new MediaTypeHeaderValue("image/jpeg");
                request.Add(fileContent);
            }

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
