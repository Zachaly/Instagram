using FluentValidation;
using Instagram.Models.DirectMessage.Request;

namespace Instagram.Application.Validation
{
    public class AddDirectMessageRequestValidator : AbstractValidator<AddDirectMessageRequest>
    {
        public AddDirectMessageRequestValidator()
        {
            RuleFor(r => r.SenderId).GreaterThan(0);

            RuleFor(r => r.ReceiverId).GreaterThan(0);

            RuleFor(r => r.Content)
                .MinimumLength(1)
                .MaximumLength(300);
        }
    }
}
