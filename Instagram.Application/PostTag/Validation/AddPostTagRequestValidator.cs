using FluentValidation;
using Instagram.Models.PostTag.Request;

namespace Instagram.Application.Validation
{
    public class AddPostTagRequestValidator : AbstractValidator<AddPostTagRequest>
    {
        public AddPostTagRequestValidator()
        {
            RuleFor(r => r.PostId)
                .GreaterThan(0);

            RuleFor(r => r.Tags)
                .MaxItemsLength(30);
        }
    }
}
