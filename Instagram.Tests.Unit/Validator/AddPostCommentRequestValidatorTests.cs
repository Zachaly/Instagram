using Instagram.Application.Validation;
using Instagram.Models.PostComment.Request;

namespace Instagram.Tests.Unit.Validator
{
    public class AddPostCommentRequestValidatorTests
    {
        private readonly AddPostCommentRequestValidator _validator;

        public AddPostCommentRequestValidatorTests()
        {
            _validator = new AddPostCommentRequestValidator();
        }

        [Fact]
        public void ValidRequest_PassesValidation()
        {
            var request = new AddPostCommentRequest
            {
                Content = "content",
                PostId = 1,
                UserId = 1,
            };

            var result = _validator.Validate(request);

            Assert.True(result.IsValid);
        }

        [Theory]
        [InlineData(0)]
        [InlineData(201)]
        public void InvalidContentLength_DoesNotPassValidation(int length)
        {
            var request = new AddPostCommentRequest
            {
                Content = new string('a', length),
                PostId = 1,
                UserId = 1,
            };

            var result = _validator.Validate(request);

            Assert.False(result.IsValid);
        }

        [Fact]
        public void InvalidPostId_DoesNotPassValidation()
        {
            var request = new AddPostCommentRequest
            {
                Content = "content",
                PostId = 0,
                UserId = 1,
            };

            var result = _validator.Validate(request);

            Assert.False(result.IsValid);
        }

        [Fact]
        public void InvalidUserId_DoesNotPassValidation()
        {
            var request = new AddPostCommentRequest
            {
                Content = "content",
                PostId = 1,
                UserId = 0,
            };

            var result = _validator.Validate(request);

            Assert.False(result.IsValid);
        }
    }
}
