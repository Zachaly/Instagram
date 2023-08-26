using Instagram.Models.PostComment;
using Instagram.Models.PostComment.Request;
using System.Net.Http.Json;

namespace Instagram.Mobile.Service
{
    public interface IPostCommentService
    {
        Task<IEnumerable<PostCommentModel>> GetAsync(GetPostCommentRequest request);
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
    }
}
