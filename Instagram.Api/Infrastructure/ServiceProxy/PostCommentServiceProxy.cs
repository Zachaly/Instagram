using FluentValidation;
using Instagram.Api.Infrastructure.ServiceProxy.Abstraction;
using Instagram.Application.Abstraction;
using Instagram.Models.PostComment;
using Instagram.Models.PostComment.Request;
namespace Instagram.Api.Infrastructure.ServiceProxy
{
    public interface IPostCommentServiceProxy : IPostCommentService { }

    public class PostCommentServiceProxy 
        : HttpLoggingServiceProxyBase<PostCommentModel, GetPostCommentRequest, AddPostCommentRequest, IPostCommentService>,
        IPostCommentServiceProxy
    {
        public PostCommentServiceProxy(ILogger<IPostCommentService> logger, IHttpContextAccessor httpContextAccessor,
            IPostCommentService postCommentService, IResponseFactory responseFactory, IValidator<AddPostCommentRequest> addValidator)
            : base(logger, httpContextAccessor, postCommentService, responseFactory, addValidator)
        {
            ServiceName = "PostComment";
        }
    }
}
