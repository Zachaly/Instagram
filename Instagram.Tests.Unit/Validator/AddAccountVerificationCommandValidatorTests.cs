using Instagram.Application.Command;
using Instagram.Application.Validation;
using Microsoft.AspNetCore.Http;
using NSubstitute;

namespace Instagram.Tests.Unit.Validator
{
    public class AddAccountVerificationCommandValidatorTests
    {
        private readonly AddAccountVerificationCommandValidator _validator;

        public AddAccountVerificationCommandValidatorTests()
        {
            _validator = new AddAccountVerificationCommandValidator();
        }

        [Fact]
        public void ValidCommand_PassesValidation()
        {
            var command = new AddAccountVerificationCommand
            {
                DateOfBirth = "21.12.2023",
                Document = Substitute.For<IFormFile>(),
                FirstName = "Fname",
                LastName = "Lname",
                UserId = 1
            };

            var res = _validator.Validate(command);

            Assert.True(res.IsValid);
        }

        [Fact]
        public void InvalidUserId_DoesNotPassValidation()
        {
            var command = new AddAccountVerificationCommand
            {
                DateOfBirth = "21.12.2023",
                Document = Substitute.For<IFormFile>(),
                FirstName = "Fname",
                LastName = "Lname",
                UserId = 0
            };

            var res = _validator.Validate(command);

            Assert.False(res.IsValid);
        }

        [Fact]
        public void DocumentNull_DoesNotPassValidation()
        {
            var command = new AddAccountVerificationCommand
            {
                DateOfBirth = "21.12.2023",
                Document = null,
                FirstName = "Fname",
                LastName = "Lname",
                UserId = 1
            };

            var res = _validator.Validate(command);

            Assert.False(res.IsValid);
        }

        [Theory]
        [InlineData(0)]
        [InlineData(11)]
        public void InvalidDateOfBirthLength_DoesNotPassValidation(int length)
        {
            var command = new AddAccountVerificationCommand
            {
                DateOfBirth = new string('a', length),
                Document = Substitute.For<IFormFile>(),
                FirstName = "Fname",
                LastName = "Lname",
                UserId = 1
            };

            var res = _validator.Validate(command);

            Assert.False(res.IsValid);
        }

        [Theory]
        [InlineData(0)]
        [InlineData(101)]
        public void InvalidFirstNameLength_DoesNotPassValidation(int length)
        {
            var command = new AddAccountVerificationCommand
            {
                DateOfBirth = "21.12.2023",
                Document = Substitute.For<IFormFile>(),
                FirstName = new string('a', length),
                LastName = "Lname",
                UserId = 1
            };

            var res = _validator.Validate(command);

            Assert.False(res.IsValid);
        }

        [Theory]
        [InlineData(0)]
        [InlineData(101)]
        public void InvalidLastNameLength_DoesNotPassValidation(int length)
        {
            var command = new AddAccountVerificationCommand
            {
                DateOfBirth = "21.12.2023",
                Document = Substitute.For<IFormFile>(),
                FirstName = "FName",
                LastName = new string('a', length),
                UserId = 1
            };

            var res = _validator.Validate(command);

            Assert.False(res.IsValid);
        }
    }
}
