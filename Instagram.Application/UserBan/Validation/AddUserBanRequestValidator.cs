using FluentValidation;
using Instagram.Models.UserBan.Request;

namespace Instagram.Application.Validation
{
    public class AddUserBanRequestValidator : AbstractValidator<AddUserBanRequest>
    {
        public AddUserBanRequestValidator()
        {
            RuleFor(r => r.UserId)
                .GreaterThan(0);

            RuleFor(r => r.EndDate)
                .GreaterThan(DateTimeOffset.UtcNow.ToUnixTimeMilliseconds());
        }
    }
}
