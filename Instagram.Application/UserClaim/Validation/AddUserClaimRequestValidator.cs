using FluentValidation;
using Instagram.Models.UserClaim.Request;

namespace Instagram.Application.Validation
{
    public class AddUserClaimRequestValidator : AbstractValidator<AddUserClaimRequest>
    {
        public AddUserClaimRequestValidator()
        {
            RuleFor(r => r.UserId)
                .GreaterThan(0);

            RuleFor(r => r.Value)
                .NotEmpty()
                .MaximumLength(30);
        }
    }
}
