using FluentValidation;
using Instagram.Application.Abstraction;
using Instagram.Models.Response;
using Instagram.Models.UserFollow;
using Instagram.Models.UserFollow.Request;

namespace Instagram.Api.Infrastructure.ServiceProxy
{
    public interface IUserFollowServiceProxy : IUserFollowService { }

    public class UserFollowServiceProxy : HttpLoggingKeylessServiceProxyBase<UserFollowModel, GetUserFollowRequest, IUserFollowService>, IUserFollowServiceProxy
    {
        private readonly IResponseFactory _responseFactory;
        private readonly IValidator<AddUserFollowRequest> _addValidator;

        public UserFollowServiceProxy(ILogger<IUserFollowService> logger, IHttpContextAccessor httpContextAccessor,
            IUserFollowService userFollowService, IResponseFactory responseFactory, IValidator<AddUserFollowRequest> addValidator)
            : base(logger, httpContextAccessor, userFollowService)
        {
            _responseFactory = responseFactory;
            _addValidator = addValidator;
        }

        public async Task<ResponseModel> AddAsync(AddUserFollowRequest request)
        {
            LogInformation("Add");

            var validation = _addValidator.Validate(request);

            var response = validation.IsValid ? await _service.AddAsync(request) : _responseFactory.CreateValidationError(validation);

            LogResponse(response, "Add");

            return response;
        }

        public async Task<ResponseModel> DeleteAsync(long followerId, long followedId)
        {
            LogInformation("Delete");

            var response = await _service.DeleteAsync(followerId, followedId);

            LogResponse(response, "Delete");

            return response;
        }
    }
}
