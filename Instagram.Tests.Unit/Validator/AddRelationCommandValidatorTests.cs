using Instagram.Application.Command;
using Instagram.Application.Validation;
using Microsoft.AspNetCore.Http;
using Moq;

namespace Instagram.Tests.Unit.Validator
{
    public class AddRelationCommandValidatorTests
    {
        private readonly AddRelationCommandValidator _validator;

        public AddRelationCommandValidatorTests()
        {
            _validator = new AddRelationCommandValidator();
        }

        private IEnumerable<IFormFile> MockFiles()
            => new List<IFormFile>() { new Mock<IFormFile>().Object };

        [Fact]
        public void ValidCommand_PassesValidation()
        {
            var command = new AddRelationCommand
            {
                UserId = 1,
                Name = "relation",
                Files = MockFiles()
            };

            var res = _validator.Validate(command);

            Assert.True(res.IsValid);
        }

        [Fact]
        public void InvalidUserId_DoesNotPassValidation()
        {
            var command = new AddRelationCommand
            {
                UserId = 0,
                Name = "relation",
                Files = MockFiles()
            };

            var res = _validator.Validate(command);

            Assert.False(res.IsValid);
        }

        [Theory]
        [InlineData(4)]
        [InlineData(51)]
        public void InvalidNameLength_DoesNotPassValidation(int length)
        {
            var command = new AddRelationCommand
            {
                UserId = 1,
                Name = new string('a', length),
                Files = MockFiles()
            };

            var res = _validator.Validate(command);

            Assert.False(res.IsValid);
        }

        [Fact]
        public void FilesEmpty_DoesNotPassValidation()
        {
            var command = new AddRelationCommand
            {
                UserId = 1,
                Name = "relation",
                Files = new List<IFormFile>()
            };

            var res = _validator.Validate(command);

            Assert.False(res.IsValid);
        }
    }
}
