using FluentValidation;
using Instagram.Application.Command;

namespace Instagram.Application.Validation
{
    public class ResolveAccountVerificationCommandValidator : AbstractValidator<ResolveAccountVerificationCommand>
    {
        public ResolveAccountVerificationCommandValidator()
        {
            RuleFor(c => c.Id).GreaterThan(0);
        }
    }
}
