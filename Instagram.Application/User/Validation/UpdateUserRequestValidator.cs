using FluentValidation;
using Instagram.Models.User.Request;

namespace Instagram.Application.Validation
{
    public class UpdateUserRequestValidator : AbstractValidator<UpdateUserRequest>
    {
        public UpdateUserRequestValidator()
        {
            RuleFor(r => r.Id)
                .GreaterThan(0);

            RuleFor(c => c.Name)
                .MaximumLength(50)
                .MinimumLength(5);

            RuleFor(c => c.Nickname)
                .MinimumLength(5)
                .MaximumLength(25);

            RuleFor(c => c.Bio)
                .MaximumLength(200);
        }
    }
}
