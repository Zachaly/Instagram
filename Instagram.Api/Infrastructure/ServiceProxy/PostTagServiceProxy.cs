using FluentValidation;
using Instagram.Application.Abstraction;
using Instagram.Models.PostTag;
using Instagram.Models.PostTag.Request;
using Instagram.Models.Response;

namespace Instagram.Api.Infrastructure.ServiceProxy
{
    public interface IPostTagServiceProxy : IPostTagService { }
    public class PostTagServiceProxy : HttpLoggingKeylessServiceProxyBase<PostTagModel, GetPostTagRequest, IPostTagService>, IPostTagServiceProxy
    {
        private readonly IResponseFactory _responseFactory;
        private readonly IValidator<AddPostTagRequest> _addValidator;

        public PostTagServiceProxy(ILogger<IPostTagService> logger, IHttpContextAccessor httpContextAccessor,
            IPostTagService postTagService, IResponseFactory responseFactory, IValidator<AddPostTagRequest> addValidator)
            : base(logger, httpContextAccessor, postTagService)
        {
            _responseFactory = responseFactory;
            _addValidator = addValidator;
        }

        public async Task<ResponseModel> AddAsync(AddPostTagRequest request)
        {
            LogInformation("Add");

            var validation = _addValidator.Validate(request);

            var response = validation.IsValid ? await _service.AddAsync(request) : _responseFactory.CreateValidationError(validation);

            LogResponse(response);

            return response;
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
