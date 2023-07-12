using FluentValidation;
using Instagram.Application.Command;

namespace Instagram.Application.Validation
{
    public class AddRelationImageCommandValidator : AbstractValidator<AddRelationImageCommand>
    {
        public AddRelationImageCommandValidator()
        {
            RuleFor(c => c.RelationId).GreaterThan(0);
            RuleFor(c => c.File).NotNull();
        }
    }
}
