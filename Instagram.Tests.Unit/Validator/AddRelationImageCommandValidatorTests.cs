using Instagram.Application.Command;
using Instagram.Application.Validation;
using Microsoft.AspNetCore.Http;
using NSubstitute;

namespace Instagram.Tests.Unit.Validator
{
    public class AddRelationImageCommandValidatorTests
    {
        private readonly AddRelationImageCommandValidator _validator;

        public AddRelationImageCommandValidatorTests()
        {
            _validator = new AddRelationImageCommandValidator();
        }

        [Fact]
        public void ValidCommand_PassesValidation()
        {
            var command = new AddRelationImageCommand
            {
                File = Substitute.For<IFormFile>(),
                RelationId = 1
            };

            var res = _validator.Validate(command);

            Assert.True(res.IsValid);
        }

        [Fact]
        public void FileNull_DoesNotPassValidation()
        {
            var command = new AddRelationImageCommand
            {
                File = null,
                RelationId = 1
            };

            var res = _validator.Validate(command);

            Assert.False(res.IsValid);
        }

        [Fact]
        public void InvalidRelationId_DoesNotPassValidation()
        {
            var command = new AddRelationImageCommand
            {
                File = Substitute.For<IFormFile>(),
                RelationId = 0
            };

            var res = _validator.Validate(command);

            Assert.False(res.IsValid);
        }
    }
}
