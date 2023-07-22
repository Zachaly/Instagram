using Instagram.Application.Validation;
using Instagram.Models.Notification.Request;

namespace Instagram.Tests.Unit.Validator
{
    public class AddNotificationRequestValidatorTests
    {
        private readonly AddNotificationRequestValidator _validator;

        public AddNotificationRequestValidatorTests()
        {
            _validator = new AddNotificationRequestValidator();
        }

        [Fact]
        public void ValidRequest_PassesValidation()
        {
            var request = new AddNotificationRequest
            {
                UserId = 1,
                Message = "msg"
            };

            var res = _validator.Validate(request);

            Assert.True(res.IsValid);
        }

        [Fact]
        public void InvalidUserId_DoesNotPassValidation()
        {
            var request = new AddNotificationRequest
            {
                UserId = 0,
                Message = "msg"
            };

            var res = _validator.Validate(request);

            Assert.False(res.IsValid);
        }

        [Theory]
        [InlineData(0)]
        [InlineData(301)]
        public void InvalidMessageLength_DoesNotPassValidation(int length)
        {
            var request = new AddNotificationRequest
            {
                UserId = 1,
                Message = new string('a', length)
            };

            var res = _validator.Validate(request);

            Assert.False(res.IsValid);
        }
    }
}
