using Instagram.Application.Command;
using Instagram.Application.Validation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Instagram.Tests.Unit.Validator
{
    public class ChangePasswordCommandValidatorTests
    {
        private readonly ChangePasswordCommandValidator _validator;

        public ChangePasswordCommandValidatorTests()
        {
            _validator = new ChangePasswordCommandValidator();
        }

        [Fact]
        public void ValidCommand_PassesValidation()
        {
            var command = new ChangePasswordCommand
            {
                NewPassword = "password",
                UserId = 1,
            };

            var result = _validator.Validate(command);

            Assert.True(result.IsValid);
        }

        [Fact]
        public void InvalidUserId_DoesNotPassValidation()
        {
            var command = new ChangePasswordCommand
            {
                NewPassword = "password",
                UserId = 0,
            };

            var result = _validator.Validate(command);

            Assert.False(result.IsValid);
        }

        [Theory]
        [InlineData(5)]
        [InlineData(51)]
        public void InvalidNewPasswordLength_DoesNotPassValidation(int length)
        {
            var command = new ChangePasswordCommand
            {
                NewPassword = new string('a', length),
                UserId = 1,
            };

            var result = _validator.Validate(command);

            Assert.False(result.IsValid);
        }
    }
}
