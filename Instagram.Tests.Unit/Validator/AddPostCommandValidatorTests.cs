using Instagram.Application.Command;
using Instagram.Application.Validation;
using Microsoft.AspNetCore.Http;
using Moq;

namespace Instagram.Tests.Unit.Validator
{
    public class AddPostCommandValidatorTests
    {
        private readonly AddPostCommandValidator _validator;

        public AddPostCommandValidatorTests()
        {
            _validator = new AddPostCommandValidator();
        }

        private IEnumerable<IFormFile> MockFiles()
            => new List<IFormFile>() { new Mock<IFormFile>().Object } ;

        [Fact]
        public void ValidCommand_PassesValidation()
        {
            var command = new AddPostCommand
            {
                Content = "fun content",
                CreatorId = 1,
                Files = MockFiles(),
            };

            var result = _validator.Validate(command);

            Assert.True(result.IsValid);
        }

        [Theory]
        [InlineData(4)]
        [InlineData(201)]
        public void InvalidContentLength_DoesNotPassValidation(int length)
        {
            var command = new AddPostCommand 
            { 
                Content = new string('a', length),
                CreatorId = 1,
                Files = MockFiles(),
            };

            var result = _validator.Validate(command);

            Assert.False(result.IsValid);
        }

        [Fact]
        public void InvalidCreatorId_DoesNotPassValidation()
        {
            var command = new AddPostCommand
            {
                Content = "content",
                CreatorId = 0,
                Files = MockFiles(),
            };

            var result = _validator.Validate(command);

            Assert.False(result.IsValid);
        }

        [Fact]
        public void EmptyFiles_DoesNotPassValidation()
        {
            var command = new AddPostCommand
            {
                Content = "content",
                CreatorId = 1,
                Files = Enumerable.Empty<IFormFile>(),
            };

            var result = _validator.Validate(command);

            Assert.False(result.IsValid);
        }

        [Fact]
        public void InvalidTagLength_DoesNotPassValidation()
        {
            var command = new AddPostCommand
            {
                Content = "content",
                CreatorId = 1,
                Files = MockFiles(),
                Tags = new string[] { new string('a', 31), new string('a', 20) }
            };

            var result = _validator.Validate(command);

            Assert.False(result.IsValid);
        }
    }
}
