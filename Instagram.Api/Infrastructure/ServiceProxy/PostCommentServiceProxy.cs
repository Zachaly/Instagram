using FluentValidation;
using Instagram.Application.Abstraction;
using Instagram.Models.PostComment;
using Instagram.Models.PostComment.Request;
using Instagram.Models.Response;

namespace Instagram.Api.Infrastructure.ServiceProxy
{
    public interface IPostCommentServiceProxy : IPostCommentService { }

    public class PostCommentServiceProxy : HttpLoggingServiceProxyBase<PostCommentModel, GetPostCommentRequest, IPostCommentService>, IPostCommentServiceProxy
    {
        private readonly IResponseFactory _responseFactory;
        private readonly IValidator<AddPostCommentRequest> _addValidator;

        public PostCommentServiceProxy(ILogger<IPostCommentService> logger, IHttpContextAccessor httpContextAccessor,
            IPostCommentService postCommentService, IResponseFactory responseFactory, IValidator<AddPostCommentRequest> addValidator)
            : base(logger, httpContextAccessor, postCommentService)
        {
            _responseFactory = responseFactory;
            _addValidator = addValidator;
            ServiceName = "PostComment";
        }

        public async Task<ResponseModel> AddAsync(AddPostCommentRequest request)
        {
            LogInformation("Add");

            var validation = _addValidator.Validate(request);

            var response = validation.IsValid ? await _service.AddAsync(request) : _responseFactory.CreateValidationError(validation);

            LogResponse(response, "Add");

            return response;
        }
    }
}
