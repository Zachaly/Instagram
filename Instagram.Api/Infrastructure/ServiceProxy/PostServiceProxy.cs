using Instagram.Application.Abstraction;
using Instagram.Models.Post;
using Instagram.Models.Post.Request;

namespace Instagram.Api.Infrastructure.ServiceProxy
{
    public interface IPostServiceProxy : IPostService { }

    public class PostServiceProxy : HttpLoggingProxyBase<IPostService>, IPostServiceProxy
    {
        private readonly IPostService _postService;

        public PostServiceProxy(ILogger<IPostService> logger, IHttpContextAccessor httpContextAccessor,
            IPostService postService) : base(logger, httpContextAccessor)
        {
            _postService = postService;
        }

        public Task<IEnumerable<PostModel>> GetAsync(GetPostRequest request)
        {
            LogInformation("Get");

            return _postService.GetAsync(request);
        }

        public Task<PostModel> GetByIdAsync(long id)
        {
            LogInformation("Get By Id");

            return _postService.GetByIdAsync(id);
        }

        public Task<int> GetCountAsync(GetPostRequest request)
        {
            LogInformation("Get Count");

            return _postService.GetCountAsync(request);
        }
    }
}
