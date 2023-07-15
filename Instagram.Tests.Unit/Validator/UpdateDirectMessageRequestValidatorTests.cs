using Instagram.Application.Validation;
using Instagram.Models.DirectMessage.Request;

namespace Instagram.Tests.Unit.Validator
{
    public class UpdateDirectMessageRequestValidatorTests
    {
        private readonly UpdateDirectMessageRequestValidator _validator;

        public UpdateDirectMessageRequestValidatorTests()
        {
            _validator = new UpdateDirectMessageRequestValidator();
        }

        [Fact]
        public void ValidRequest_PassesValidation()
        {
            var request = new UpdateDirectMessageRequest
            {
                Id = 1,
                Content = "content",
                Read = true
            };

            var res = _validator.Validate(request);

            Assert.True(res.IsValid);
        }

        [Fact]
        public void ValidRequest_OnlyRequiredFields_PassesValidation()
        {
            var request = new UpdateDirectMessageRequest
            {
                Id = 1,
            };

            var res = _validator.Validate(request);

            Assert.True(res.IsValid);
        }

        [Fact]
        public void InvalidId_DoesNotPassValidation()
        {
            var request = new UpdateDirectMessageRequest
            {
                Id = 0
            };

            var res = _validator.Validate(request);

            Assert.False(res.IsValid);
        }

        [Theory]
        [InlineData(0)]
        [InlineData(301)]
        public void InvalidContentLength_DoesNotPassValidation(int length)
        {
            var request = new UpdateDirectMessageRequest
            {
                Id = 1,
                Content = new string('a', length)
            };

            var res = _validator.Validate(request);

            Assert.False(res.IsValid);
        }

    }
}
