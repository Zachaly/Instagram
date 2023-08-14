using FluentValidation;
using Instagram.Api.Infrastructure.NotificationCommands;
using Instagram.Application.Abstraction;
using Instagram.Models.Response;
using Instagram.Models.UserFollow;
using Instagram.Models.UserFollow.Request;
using MediatR;

namespace Instagram.Api.Infrastructure.ServiceProxy
{
    public interface IUserFollowServiceProxy : IUserFollowService { }

    public class UserFollowServiceProxy : HttpLoggingKeylessServiceProxyBase<UserFollowModel, GetUserFollowRequest, IUserFollowService>, IUserFollowServiceProxy
    {
        private readonly IResponseFactory _responseFactory;
        private readonly IValidator<AddUserFollowRequest> _addValidator;
        private readonly IMediator _mediator;

        public UserFollowServiceProxy(ILogger<IUserFollowService> logger, IHttpContextAccessor httpContextAccessor,
            IUserFollowService userFollowService, IResponseFactory responseFactory, IValidator<AddUserFollowRequest> addValidator,
            IMediator mediator)
            : base(logger, httpContextAccessor, userFollowService)
        {
            _responseFactory = responseFactory;
            _addValidator = addValidator;
            _mediator = mediator;
            ServiceName = "UserFollow";
        }

        public async Task<ResponseModel> AddAsync(AddUserFollowRequest request)
        {
            LogInformation("Add");

            var validation = _addValidator.Validate(request);

            var response = validation.IsValid ? await _service.AddAsync(request) : _responseFactory.CreateValidationError(validation);

            LogResponse(response, "Add");

            if (response.Success)
            {
                await _mediator.Send(new SendFollowNotificationCommand
                {
                    FollowedUserId = request.FollowedUserId,
                    FollowerId = request.FollowingUserId
                });
            }

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
