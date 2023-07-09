using FluentValidation;
using Instagram.Application.Command;

namespace Instagram.Application.Validation
{
    public class AddPostCommandValidator : AbstractValidator<AddPostCommand>
    {
        public AddPostCommandValidator()
        {
            RuleFor(c => c.Content)
                .NotEmpty()
                .MinimumLength(5)
                .MaximumLength(200);

            RuleFor(c => c.Files)
                .NotEmpty();

            RuleFor(c => c.CreatorId)
                .GreaterThan(0);

            RuleFor(c => c.Tags)
                .MaxItemsLength(30)
                .When(r => r.Tags is not null);
        }
    }
}
