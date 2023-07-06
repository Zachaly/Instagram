using FluentValidation;
using Instagram.Application.Command;

namespace Instagram.Application.Validation
{
    public class ChangePasswordCommandValidator : AbstractValidator<ChangePasswordCommand>
    {
        public ChangePasswordCommandValidator()
        {
            RuleFor(c => c.NewPassword)
                .MaximumLength(50)
                .MinimumLength(6);

            RuleFor(c => c.UserId)
                .GreaterThan(0);
        }
    }
}
