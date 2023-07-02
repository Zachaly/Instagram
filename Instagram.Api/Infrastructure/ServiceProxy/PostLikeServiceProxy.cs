using Instagram.Application.Abstraction;
using Instagram.Models.PostLike;
using Instagram.Models.PostLike.Request;
using Instagram.Models.Response;

namespace Instagram.Api.Infrastructure.ServiceProxy
{
    public interface IPostLikeServiceProxy : IPostLikeService { }

    public class PostLikeServiceProxy : HttpLoggingKeylessServiceProxyBase<PostLikeModel, GetPostLikeRequest, IPostLikeService>, IPostLikeServiceProxy
    {
        public PostLikeServiceProxy(ILogger<IPostLikeService> logger, IHttpContextAccessor httpContextAccessor,
            IPostLikeService postLikeService) : base(logger, httpContextAccessor, postLikeService)
        {
        }

        public async Task<ResponseModel> AddAsync(AddPostLikeRequest request)
        {
            LogInformation("Add");

            var response = await _service.AddAsync(request);

            LogResponse(response, "Add");

            return response;
        }

        public async Task<ResponseModel> DeleteAsync(long postId, long userId)
        {
            LogInformation("Delete");

            var response = await _service.DeleteAsync(postId, userId);

            LogResponse(response, "Delete");

            return response;
        }
    }
}
