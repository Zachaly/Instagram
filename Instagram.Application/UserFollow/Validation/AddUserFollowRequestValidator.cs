using FluentValidation;
using Instagram.Models.UserFollow.Request;

namespace Instagram.Application.Validation
{
    public class AddUserFollowRequestValidator : AbstractValidator<AddUserFollowRequest>
    {
        public AddUserFollowRequestValidator()
        {
            RuleFor(r => r.FollowedUserId)
                .GreaterThan(0);

            RuleFor(r => r.FollowingUserId)
                .GreaterThan(0);
        }
    }
}
