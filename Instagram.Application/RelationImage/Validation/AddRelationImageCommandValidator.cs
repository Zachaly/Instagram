using FluentValidation;
using Instagram.Application.Command;

namespace Instagram.Application.Validation
{
    public class AddRelationImageCommandValidator : AbstractValidator<AddRelationImageCommand>
    {
        public AddRelationImageCommandValidator()
        {

        }
    }
}
