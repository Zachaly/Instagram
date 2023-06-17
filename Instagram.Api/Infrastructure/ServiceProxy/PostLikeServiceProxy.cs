using Instagram.Application.Abstraction;
using Instagram.Models.PostLike;
using Instagram.Models.PostLike.Request;
using Instagram.Models.Response;

namespace Instagram.Api.Infrastructure.ServiceProxy
{
    public interface IPostLikeServiceProxy : IPostLikeService { }

    public class PostLikeServiceProxy : HttpLoggingProxyBase<IPostLikeService>, IPostLikeServiceProxy
    {
        private readonly IPostLikeService _postLikeService;

        public PostLikeServiceProxy(ILogger<IPostLikeService> logger, IHttpContextAccessor httpContextAccessor,
            IPostLikeService postLikeService) : base(logger, httpContextAccessor)
        {
            _postLikeService = postLikeService;
        }

        public async Task<ResponseModel> AddAsync(AddPostLikeRequest request)
        {
            LogInformation("Add");

            var response = await _postLikeService.AddAsync(request);

            LogResponse(response, "Add");

            return response;
        }

        public async Task<ResponseModel> DeleteAsync(long postId, long userId)
        {
            LogInformation("Delete");

            var response = await _postLikeService.DeleteAsync(postId, userId);

            LogResponse(response, "Delete");

            return response;
        }

        public Task<IEnumerable<PostLikeModel>> GetAsync(GetPostLikeRequest request)
        {
            LogInformation("Get");

            return _postLikeService.GetAsync(request);
        }

        public Task<int> GetCountAsync(GetPostLikeRequest request)
        {
            LogInformation("Get Count");

            return _postLikeService.GetCountAsync(request);
        }
    }
}
