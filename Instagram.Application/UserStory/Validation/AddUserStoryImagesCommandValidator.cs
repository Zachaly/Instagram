using FluentValidation;
using Instagram.Application.Command;

namespace Instagram.Application.Validation
{
    public class AddUserStoryImagesCommandValidator : AbstractValidator<AddUserStoryImagesCommand>
    {
        public AddUserStoryImagesCommandValidator()
        {
            RuleFor(c => c.Images).NotEmpty();
            RuleFor(c => c.UserId).GreaterThan(0);
        }
    }
}
