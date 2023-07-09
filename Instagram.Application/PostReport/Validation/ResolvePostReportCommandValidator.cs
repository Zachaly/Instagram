using FluentValidation;
using Instagram.Application.Command;

namespace Instagram.Application.Validation
{
    public class ResolvePostReportCommandValidator : AbstractValidator<ResolvePostReportCommand>
    {
        public ResolvePostReportCommandValidator()
        {
            RuleFor(c => c.Id)
                .GreaterThan(0);

            RuleFor(c => c.UserId)
                .GreaterThan(0);

            RuleFor(c => c.BanEndDate)
                .GreaterThan(DateTimeOffset.UtcNow.ToUnixTimeMilliseconds());

            RuleFor(c => c.PostId)
                .GreaterThan(0);
        }
    }
}
