using FluentValidation;
using Instagram.Api.Infrastructure.NotificationCommands;
using Instagram.Api.Infrastructure.ServiceProxy.Abstraction;
using Instagram.Application.Abstraction;
using Instagram.Models.Response;
using Instagram.Models.UserFollow;
using Instagram.Models.UserFollow.Request;
using MediatR;

namespace Instagram.Api.Infrastructure.ServiceProxy
{
    public interface IUserFollowServiceProxy : IUserFollowService { }

    public class UserFollowServiceProxy 
        : HttpLoggingKeylessServiceProxyBase<UserFollowModel, GetUserFollowRequest, AddUserFollowRequest, IUserFollowService>,
        IUserFollowServiceProxy
    {
        private readonly IMediator _mediator;

        public UserFollowServiceProxy(ILogger<IUserFollowService> logger, IHttpContextAccessor httpContextAccessor,
            IUserFollowService userFollowService, IResponseFactory responseFactory, IValidator<AddUserFollowRequest> addValidator,
            IMediator mediator)
            : base(logger, httpContextAccessor, userFollowService, responseFactory, addValidator)
        {
            _mediator = mediator;
            ServiceName = "UserFollow";
        }

        public override async Task<ResponseModel> AddAsync(AddUserFollowRequest request)
        {
            var response = await base.AddAsync(request);

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
