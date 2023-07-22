using FluentValidation;
using Instagram.Models.Notification.Request;

namespace Instagram.Application.Validation
{
    public class UpdateNotificationRequestValidator : AbstractValidator<UpdateNotificationRequest>
    {
        public UpdateNotificationRequestValidator()
        {
            RuleFor(r => r.Id).GreaterThan(0);
        }
    }
}
