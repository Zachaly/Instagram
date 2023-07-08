using Instagram.Application.Validation;
using Instagram.Models.PostLike.Request;

namespace Instagram.Tests.Unit.Validator
{
    public class AddPostLikeRequestValidatorTests
    {
        private readonly AddPostLikeRequestValidator _validator;

        public AddPostLikeRequestValidatorTests()
        {
            _validator = new AddPostLikeRequestValidator();
        }

        [Fact]
        public void ValidRequest_PassesValidation()
        {
            var request = new AddPostLikeRequest
            {
                PostId = 1,
                UserId = 1,
            };

            var result = _validator.Validate(request);

            Assert.True(result.IsValid);
        }

        [Fact]
        public void InvalidUserId_DoesNotPassValidation()
        {
            var request = new AddPostLikeRequest
            {
                PostId = 1,
                UserId = 0
            };

            var result = _validator.Validate(request);

            Assert.False(result.IsValid);
        }

        [Fact]
        public void InvalidPostId_DoesNotPassValidation()
        {
            var request = new AddPostLikeRequest
            {
                PostId = 0,
                UserId = 1,
            };

            var result = _validator.Validate(request);

            Assert.False(result.IsValid);
        }
    }
}
