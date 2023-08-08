using FluentValidation;
using Instagram.Models.UserBlock.Request;

namespace Instagram.Application.Validation
{
    public class AddUserBlockRequestValidator : AbstractValidator<AddUserBlockRequest>
    {
        public AddUserBlockRequestValidator()
        {
            RuleFor(r => r.BlockedUserId).GreaterThan(0);
            RuleFor(r => r.BlockingUserId).GreaterThan(0);
        }
    }
}
