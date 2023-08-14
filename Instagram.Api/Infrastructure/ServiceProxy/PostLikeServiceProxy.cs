using FluentValidation;
using Instagram.Application.Abstraction;
using Instagram.Models.PostLike;
using Instagram.Models.PostLike.Request;
using Instagram.Models.Response;

namespace Instagram.Api.Infrastructure.ServiceProxy
{
    public interface IPostLikeServiceProxy : IPostLikeService { }

    public class PostLikeServiceProxy : HttpLoggingKeylessServiceProxyBase<PostLikeModel, GetPostLikeRequest, IPostLikeService>, IPostLikeServiceProxy
    {
        private readonly IResponseFactory _responseFactory;
        private readonly IValidator<AddPostLikeRequest> _addValidator;

        public PostLikeServiceProxy(ILogger<IPostLikeService> logger, IHttpContextAccessor httpContextAccessor,
            IPostLikeService postLikeService, IResponseFactory responseFactory, IValidator<AddPostLikeRequest> addValidator)
            : base(logger, httpContextAccessor, postLikeService)
        {
            _responseFactory = responseFactory;
            _addValidator = addValidator;
            ServiceName = "PostLike";
        }

        public async Task<ResponseModel> AddAsync(AddPostLikeRequest request)
        {
            LogInformation("Add");

            var validation = _addValidator.Validate(request);

            var response = validation.IsValid ? await _service.AddAsync(request) : _responseFactory.CreateValidationError(validation);

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
