using FluentValidation;
using Instagram.Models.PostReport.Request;

namespace Instagram.Application.Validation
{
    public class AddPostReportRequestValidator : AbstractValidator<AddPostReportRequest>
    {
        public AddPostReportRequestValidator()
        {
            RuleFor(r => r.ReportingUserId)
                .GreaterThan(0);

            RuleFor(r => r.PostId)
                .GreaterThan(0);

            RuleFor(r => r.Reason)
                .MinimumLength(10)
                .MaximumLength(200);
        }
    }
}
