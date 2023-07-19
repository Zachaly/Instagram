using Instagram.Application.Command;
using Instagram.Application.Validation;

namespace Instagram.Tests.Unit.Validator
{
    public class ResolveAccountVerificationCommandValidatorTests
    {
        private readonly ResolveAccountVerificationCommandValidator _validator;

        public ResolveAccountVerificationCommandValidatorTests()
        {
            _validator = new ResolveAccountVerificationCommandValidator();
        }

        [Fact]
        public void ValidCommand_PassesValidation()
        {
            var command = new ResolveAccountVerificationCommand
            {
                Accepted = true,
                Id = 1
            };

            var res = _validator.Validate(command);

            Assert.True(res.IsValid);
        }

        [Fact]
        public void InvalidId_DoesNotPassValidation()
        {
            var command = new ResolveAccountVerificationCommand
            {
                Accepted = true,
                Id = 0
            };

            var res = _validator.Validate(command);

            Assert.False(res.IsValid);
        }
    }
}
