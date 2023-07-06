using FluentValidation;
using Instagram.Models.PostComment.Request;

namespace Instagram.Application.Validation
{
    public class AddPostCommentRequestValidator : AbstractValidator<AddPostCommentRequest>
    {
        public AddPostCommentRequestValidator()
        {
            RuleFor(r => r.Content)
                .NotEmpty()
                .MaximumLength(200);

            RuleFor(r => r.UserId)
                .GreaterThan(0);

            RuleFor(r => r.PostId)
                .GreaterThan(0);
        }
    }
}
