using FluentValidation;
using Instagram.Models.UserBlock.Request;

namespace Instagram.Application.Validation
{
    public class AddUserBlockRequestValidator : AbstractValidator<AddUserBlockRequest>
    {
        public AddUserBlockRequestValidator()
        {

        }
    }
}
