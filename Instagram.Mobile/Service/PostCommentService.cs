using Instagram.Models.PostComment;
using Instagram.Models.PostComment.Request;
using Instagram.Models.Response;
using Newtonsoft.Json;
using System.Net;
using System.Net.Http.Json;

namespace Instagram.Mobile.Service
{
    public interface IPostCommentService
    {
        Task<IEnumerable<PostCommentModel>> GetAsync(GetPostCommentRequest request);
        Task<long> AddAsync(AddPostCommentRequest request);
        Task<PostCommentModel> GetByIdAsync(long id);
    }

    public class PostCommentService : IPostCommentService
    {
        private readonly HttpClient _httpClient;
        private const string Endpoint = "post-comment";

        public PostCommentService(IHttpClientFactory httpClientFactory)
        {
            _httpClient = httpClientFactory.Create();
        }

        public async Task<IEnumerable<PostCommentModel>> GetAsync(GetPostCommentRequest request)
        {
            var response = await _httpClient.GetAsync(request.BuildQuery(Endpoint));

            return await response.Content.ReadFromJsonAsync<IEnumerable<PostCommentModel>>();
        }

        public async Task<long> AddAsync(AddPostCommentRequest request)
        {
            var response = await _httpClient.PostAsJsonAsync(Endpoint, request);

            if(response.StatusCode == HttpStatusCode.BadRequest)
            {
                var content = await response.Content.ReadAsStringAsync();

                throw new InvalidRequestException(JsonConvert.DeserializeObject<ResponseModel>(content));
            }

            var id = (await response.Content.ReadFromJsonAsync<ResponseModel>()).NewEntityId;

            return id.Value;
        }

        public async Task<PostCommentModel> GetByIdAsync(long id)
        {
            var response = await _httpClient.GetAsync($"{Endpoint}/{id}");

            if(response.StatusCode == HttpStatusCode.NotFound)
            {
                throw new NotFoundException("Post comment");
            }

            return await response.Content.ReadFromJsonAsync<PostCommentModel>();
        }
    }
}
