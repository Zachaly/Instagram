using FluentValidation;
using Instagram.Api.Infrastructure.ServiceProxy.Abstraction;
using Instagram.Application.Abstraction;
using Instagram.Models.PostLike;
using Instagram.Models.PostLike.Request;
using Instagram.Models.Response;

namespace Instagram.Api.Infrastructure.ServiceProxy
{
    public interface IPostLikeServiceProxy : IPostLikeService { }

    public class PostLikeServiceProxy 
        : HttpLoggingKeylessServiceProxyBase<PostLikeModel, GetPostLikeRequest, AddPostLikeRequest, IPostLikeService>,
        IPostLikeServiceProxy
    {

        public PostLikeServiceProxy(ILogger<IPostLikeService> logger, IHttpContextAccessor httpContextAccessor,
            IPostLikeService postLikeService, IResponseFactory responseFactory, IValidator<AddPostLikeRequest> addValidator)
            : base(logger, httpContextAccessor, postLikeService, responseFactory, addValidator)
        {
            ServiceName = "PostLike";
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
