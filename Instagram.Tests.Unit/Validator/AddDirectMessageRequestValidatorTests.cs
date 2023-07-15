using Instagram.Application.Validation;
using Instagram.Models.DirectMessage.Request;

namespace Instagram.Tests.Unit.Validator
{
    public class AddDirectMessageRequestValidatorTests
    {
        private readonly AddDirectMessageRequestValidator _validator;

        public AddDirectMessageRequestValidatorTests()
        {
            _validator = new AddDirectMessageRequestValidator();
        }

        [Fact]
        public void ValidRequest_PassesValidation()
        {
            var request = new AddDirectMessageRequest
            {
                SenderId = 1,
                ReceiverId = 2,
                Content = "content"
            };

            var res = _validator.Validate(request);

            Assert.True(res.IsValid);
        }

        [Fact]
        public void InvalidSenderId_DoesNotPassValidation()
        {
            var request = new AddDirectMessageRequest
            {
                SenderId = 0,
                ReceiverId = 2,
                Content = "content"
            };

            var res = _validator.Validate(request);

            Assert.False(res.IsValid);
        }

        [Fact]
        public void InvalidReceiverId_DoesNotPassValidation()
        {
            var request = new AddDirectMessageRequest
            {
                SenderId = 1,
                ReceiverId = 0,
                Content = "content"
            };

            var res = _validator.Validate(request);

            Assert.False(res.IsValid);
        }

        [Theory]
        [InlineData(0)]
        [InlineData(301)]
        public void InvalidContentLength_DoesNotPassValidation(int length)
        {
            var request = new AddDirectMessageRequest
            {
                SenderId = 1,
                ReceiverId = 2,
                Content = new string('a', length)
            };

            var res = _validator.Validate(request);

            Assert.False(res.IsValid);
        }
    }
}
