using FluentValidation;
using Instagram.Models.Notification.Request;

namespace Instagram.Application.Validation
{
    public class AddNotificationRequestValidator : AbstractValidator<AddNotificationRequest>
    {
        public AddNotificationRequestValidator()
        {
            RuleFor(r => r.UserId).GreaterThan(0);
            RuleFor(r => r.Message).MinimumLength(1).MaximumLength(300);
        }
    }
}
