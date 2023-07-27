using FluentValidation;
using Instagram.Application.Command;

namespace Instagram.Application.Validation
{
    public class AddUserStoryImagesCommandValidator : AbstractValidator<AddUserStoryImagesCommand>
    {
        public AddUserStoryImagesCommandValidator()
        {

        }
    }
}
