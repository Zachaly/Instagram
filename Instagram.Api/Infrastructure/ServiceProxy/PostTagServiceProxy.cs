using FluentValidation;
using Instagram.Api.Infrastructure.ServiceProxy.Abstraction;
using Instagram.Application.Abstraction;
using Instagram.Models.PostTag;
using Instagram.Models.PostTag.Request;
using Instagram.Models.Response;

namespace Instagram.Api.Infrastructure.ServiceProxy
{
    public interface IPostTagServiceProxy : IPostTagService { }
    public class PostTagServiceProxy : HttpLoggingKeylessServiceProxyBase<PostTagModel, GetPostTagRequest, AddPostTagRequest, IPostTagService>,
        IPostTagServiceProxy
    {
        public PostTagServiceProxy(ILogger<IPostTagService> logger, IHttpContextAccessor httpContextAccessor,
            IPostTagService postTagService, IResponseFactory responseFactory, IValidator<AddPostTagRequest> addValidator)
            : base(logger, httpContextAccessor, postTagService, responseFactory, addValidator)
        {
            ServiceName = "PostTag";
        }

        public async Task<ResponseModel> DeleteAsync(long postId, string tag)
        {
            LogInformation("Delete");

            var response = await _service.DeleteAsync(postId, tag);

            LogResponse(response);

            return response;
        }
    }
}
