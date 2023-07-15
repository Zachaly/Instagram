using FluentValidation;
using Instagram.Models.DirectMessage.Request;

namespace Instagram.Application.Validation
{
    public class UpdateDirectMessageRequestValidator : AbstractValidator<UpdateDirectMessageRequest>
    {
        public UpdateDirectMessageRequestValidator()
        {
            RuleFor(r => r.Id).GreaterThan(0);

            RuleFor(r => r.Content)
                .MinimumLength(1)
                .MaximumLength(300);
        }
    }
}
