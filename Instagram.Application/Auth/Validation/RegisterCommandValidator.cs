using FluentValidation;
using Instagram.Application.Command;

namespace Instagram.Application.Validation
{
    public class RegisterCommandValidator : AbstractValidator<RegisterCommand>
    {
        public RegisterCommandValidator()
        {
            RuleFor(c => c.Name)
                .NotEmpty()
                .MaximumLength(50)
                .MinimumLength(5);

            RuleFor(c => c.Email)
                .NotEmpty()
                .EmailAddress();

            RuleFor(c => c.Password)
                .MaximumLength(50)
                .MinimumLength(6);

            RuleFor(c => c.Nickname)
                .MinimumLength(5)
                .MaximumLength(25);
        }
    }
}
