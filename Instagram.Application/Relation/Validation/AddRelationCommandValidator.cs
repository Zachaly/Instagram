using FluentValidation;
using Instagram.Application.Command;

namespace Instagram.Application.Validation
{
    public class AddRelationCommandValidator : AbstractValidator<AddRelationCommand>
    {
        public AddRelationCommandValidator()
        {

        }
    }
}
