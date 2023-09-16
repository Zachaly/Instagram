using Instagram.Mobile.Extension;
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

        public Task<IEnumerable<PostCommentModel>> GetAsync(GetPostCommentRequest request)
            => _httpClient.GetWithRequestAsync<PostCommentModel, GetPostCommentRequest>(Endpoint, request);

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

        public Task<PostCommentModel> GetByIdAsync(long id)
            => _httpClient.GetByIdAsync<PostCommentModel>(Endpoint, id);
    }
}
