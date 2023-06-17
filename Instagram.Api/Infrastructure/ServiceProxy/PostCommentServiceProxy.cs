using Instagram.Application.Abstraction;
using Instagram.Models.PostComment;
using Instagram.Models.PostComment.Request;
using Instagram.Models.Response;

namespace Instagram.Api.Infrastructure.ServiceProxy
{
    public interface IPostCommentServiceProxy : IPostCommentService { }

    public class PostCommentServiceProxy : HttpLoggingProxyBase<IPostCommentService>, IPostCommentServiceProxy
    {
        private readonly IPostCommentService _postCommentService;

        public PostCommentServiceProxy(ILogger<IPostCommentService> logger, IHttpContextAccessor httpContextAccessor,
            IPostCommentService postCommentService) : base(logger, httpContextAccessor)
        {
            _postCommentService = postCommentService;
        }

        public async Task<ResponseModel> AddAsync(AddPostCommentRequest request)
        {
            LogInformation("Add");

            var response = await _postCommentService.AddAsync(request);

            LogResponse(response, "Add");

            return response;
        }

        public Task<IEnumerable<PostCommentModel>> GetAsync(GetPostCommentRequest request)
        {
            LogInformation("Get");

            return _postCommentService.GetAsync(request);
        }

        public Task<PostCommentModel> GetByIdAsync(long id)
        {
            LogInformation("Get By Id");

            return _postCommentService.GetByIdAsync(id);
        }

        public Task<int> GetCountAsync(GetPostCommentRequest request)
        {
            LogInformation("Get Count");

            return _postCommentService.GetCountAsync(request);
        }
    }
}
