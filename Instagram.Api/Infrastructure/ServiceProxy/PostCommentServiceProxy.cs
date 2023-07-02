using Instagram.Application.Abstraction;
using Instagram.Models.PostComment;
using Instagram.Models.PostComment.Request;
using Instagram.Models.Response;

namespace Instagram.Api.Infrastructure.ServiceProxy
{
    public interface IPostCommentServiceProxy : IPostCommentService { }

    public class PostCommentServiceProxy : HttpLoggingServiceProxyBase<PostCommentModel, GetPostCommentRequest, IPostCommentService>, IPostCommentServiceProxy
    {
        public PostCommentServiceProxy(ILogger<IPostCommentService> logger, IHttpContextAccessor httpContextAccessor,
            IPostCommentService postCommentService) : base(logger, httpContextAccessor, postCommentService)
        {
        }

        public async Task<ResponseModel> AddAsync(AddPostCommentRequest request)
        {
            LogInformation("Add");

            var response = await _service.AddAsync(request);

            LogResponse(response, "Add");

            return response;
        }
    }
}
