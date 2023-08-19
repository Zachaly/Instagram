using Instagram.Api.Infrastructure.ServiceProxy.Abstraction;
using Instagram.Application.Abstraction;
using Instagram.Models.Post;
using Instagram.Models.Post.Request;

namespace Instagram.Api.Infrastructure.ServiceProxy
{
    public interface IPostServiceProxy : IPostService { }

    public class PostServiceProxy : HttpLoggingServiceProxyBase<PostModel, GetPostRequest, IPostService>, IPostServiceProxy
    {
        public PostServiceProxy(ILogger<IPostService> logger, IHttpContextAccessor httpContextAccessor,
            IPostService postService) : base(logger, httpContextAccessor, postService)
        {
            ServiceName = "Post";
        }
    }
}
