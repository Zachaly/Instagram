using FluentValidation;
using Instagram.Application.Command;

namespace Instagram.Application.Validation
{
    public class AddAccountVerificationCommandValidator : AbstractValidator<AddAccountVerificationCommand>
    {
        public AddAccountVerificationCommandValidator()
        {
            RuleFor(c => c.UserId).GreaterThan(0);
            RuleFor(c => c.DateOfBirth).NotEmpty().MaximumLength(10);
            RuleFor(c => c.FirstName).NotEmpty().MaximumLength(100);
            RuleFor(c => c.LastName).NotEmpty().MaximumLength(100);
            RuleFor(c => c.Document).NotNull();
        }
    }
}
