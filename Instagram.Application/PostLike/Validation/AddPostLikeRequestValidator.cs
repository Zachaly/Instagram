using FluentValidation;
using Instagram.Models.PostLike.Request;

namespace Instagram.Application.Validation
{
    public class AddPostLikeRequestValidator : AbstractValidator<AddPostLikeRequest>
    {
        public AddPostLikeRequestValidator()
        {
            RuleFor(r => r.UserId)
                .GreaterThan(0);

            RuleFor(r => r.PostId)
                .GreaterThan(0);
        }
    }
}
