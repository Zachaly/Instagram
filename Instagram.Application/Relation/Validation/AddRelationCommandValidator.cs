using FluentValidation;
using Instagram.Application.Command;

namespace Instagram.Application.Validation
{
    public class AddRelationCommandValidator : AbstractValidator<AddRelationCommand>
    {
        public AddRelationCommandValidator()
        {
            RuleFor(c => c.UserId).GreaterThan(0);
            RuleFor(c => c.Files).NotEmpty();
            RuleFor(c => c.Name)
                .MaximumLength(50)
                .MinimumLength(5);
        }
    }
}
