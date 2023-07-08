using Instagram.Application.Validation;
using Instagram.Models.PostTag.Request;

namespace Instagram.Tests.Unit.Validator
{
    public class AddPostTagRequestValidatorTests
    {
        private readonly AddPostTagRequestValidator _validator;

        public AddPostTagRequestValidatorTests()
        {
            _validator = new AddPostTagRequestValidator();
        }

        [Fact]
        public void ValidRequest_PassesValidation()
        {
            var request = new AddPostTagRequest
            {
                PostId = 1,
                Tags = new List<string> { "tag1", "tag2" }
            };

            var result = _validator.Validate(request);

            Assert.True(result.IsValid);
        }

        [Fact]
        public void InvalidPostId_DoesNotPassValidation()
        {
            var request = new AddPostTagRequest
            {
                PostId = 0,
                Tags = new List<string> { "tag1", "tag2" }
            };

            var result = _validator.Validate(request);

            Assert.False(result.IsValid);
        }

        [Fact]
        public void InvalidTagLength_DoesNotPassValidation()
        {
            var request = new AddPostTagRequest
            {
                PostId = 1,
                Tags = new List<string> { "tag1", "tag2", new string('a', 31) }
            };

            var result = _validator.Validate(request);

            Assert.False(result.IsValid);
        }
    }
}
