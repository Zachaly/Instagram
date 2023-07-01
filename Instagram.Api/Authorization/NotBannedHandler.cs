using Microsoft.AspNetCore.Authorization;

namespace Instagram.Api.Authorization
{
    public class NotBannedRequirement : IAuthorizationRequirement { }

    public class NotBannedHandler : AuthorizationHandler<NotBannedRequirement>
    {
        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, NotBannedRequirement requirement)
        {
            var banClaim = context.User.Claims.FirstOrDefault(c => c.Value == UserClaimValues.Ban);

            if (banClaim is not null)
            {
                context.Fail();
            } 
            else
            {
                context.Succeed(requirement);
            }

            return Task.CompletedTask;
        }
    }
}
