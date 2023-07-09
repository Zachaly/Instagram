using Instagram.Application.Command;
using Instagram.Application.Validation;

namespace Instagram.Tests.Unit.Validator
{
    public class RegisterCommandValidatorTests
    {
        private readonly RegisterCommandValidator _validator;

        public RegisterCommandValidatorTests()
        {
            _validator = new RegisterCommandValidator();
        }

        [Fact]
        public void ValidCommand_PassesValidation()
        {
            var command = new RegisterCommand
            {
                Email = "email@email.com",
                Name = "fun name",
                Nickname = "fun nickname",
                Password = "password"
            };

            var result = _validator.Validate(command);

            Assert.True(result.IsValid);
        }

        [Fact]
        public void InvalidEmail_DoesNotPassValidation()
        {
            var command = new RegisterCommand
            {
                Email = "email",
                Name = "fun name",
                Nickname = "fun nickname",
                Password = "password"
            };

            var result = _validator.Validate(command);

            Assert.False(result.IsValid);
        }

        [Theory]
        [InlineData(4)]
        [InlineData(51)]
        public void InvalidNameLength_DoesNotPassValidation(int length)
        {
            var command = new RegisterCommand
            {
                Email = "email@email.com",
                Name = new string('a', length),
                Nickname = "fun nickname",
                Password = "password"
            };

            var result = _validator.Validate(command);

            Assert.False(result.IsValid);
        }

        [Theory]
        [InlineData(4)]
        [InlineData(26)]
        public void InvalidNicknameLength_DoesNotPassValidation(int length)
        {
            var command = new RegisterCommand
            {
                Email = "email@email.com",
                Name = "fun name",
                Nickname = new string('a', length),
                Password = "password"
            };

            var result = _validator.Validate(command);

            Assert.False(result.IsValid);
        }

        [Theory]
        [InlineData(5)]
        [InlineData(51)]
        public void InvalidPasswordLength_DoesNotPassValidation(int length)
        {
            var command = new RegisterCommand
            {
                Email = "email@email.com",
                Name = "fun name",
                Nickname = "fun nickname",
                Password = new string('a', length)
            };

            var result = _validator.Validate(command);

            Assert.False(result.IsValid);
        }
    }
}
