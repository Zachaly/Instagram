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

        public Task<ResponseModel> AddAsync(AddPostTagRequest request)
        {
            throw new NotImplementedException();
        }

        public Task<ResponseModel> DeleteAsync(long postId, string tag)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<PostTagModel>> GetAsync(GetPostTagRequest request)
        {
            throw new NotImplementedException();
        }

        public Task<int> GetCountAsync(GetPostTagRequest request)
        {
            throw new NotImplementedException();
        }
    }
}
