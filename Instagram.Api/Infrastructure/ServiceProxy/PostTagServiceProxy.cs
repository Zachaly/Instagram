using Instagram.Application.Abstraction;
using Instagram.Models.PostTag;
using Instagram.Models.PostTag.Request;
using Instagram.Models.Response;

namespace Instagram.Api.Infrastructure.ServiceProxy
{
    public interface IPostTagServiceProxy : IPostTagService { }
    public class PostTagServiceProxy : HttpLoggingProxyBase<IPostTagService>, IPostTagServiceProxy
    {
        private readonly IPostTagService _postTagService;

        public PostTagServiceProxy(ILogger<IPostTagService> logger, IHttpContextAccessor httpContextAccessor,
            IPostTagService postTagService) : base(logger, httpContextAccessor)
        {
            _postTagService = postTagService;
        }

        public async Task<ResponseModel> AddAsync(AddPostTagRequest request)
        {
            LogInformation("Add");

            var response = await _postTagService.AddAsync(request);

            LogResponse(response);

            return response;
        }

        public async Task<ResponseModel> DeleteAsync(long postId, string tag)
        {
            LogInformation("Delete");

            var response = await _postTagService.DeleteAsync(postId, tag);

            LogResponse(response);

            return response;
        }

        public Task<IEnumerable<PostTagModel>> GetAsync(GetPostTagRequest request)
        {
            LogInformation("Get");

            return _postTagService.GetAsync(request);
        }

        public Task<int> GetCountAsync(GetPostTagRequest request)
        {
            LogInformation("Get Count");

            return _postTagService.GetCountAsync(request);
        }
    }
}
