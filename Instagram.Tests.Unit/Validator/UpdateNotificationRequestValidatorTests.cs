using Instagram.Application.Validation;
using Instagram.Models.Notification.Request;

namespace Instagram.Tests.Unit.Validator
{
    public class UpdateNotificationRequestValidatorTests
    {
        private readonly UpdateNotificationRequestValidator _validator;

        public UpdateNotificationRequestValidatorTests()
        {
            _validator = new UpdateNotificationRequestValidator();
        }

        [Fact]
        public void ValidRequest_PassesValidation()
        {
            var request = new UpdateNotificationRequest
            {
                Id = 1,
                IsRead = true,
            };

            var res = _validator.Validate(request);

            Assert.True(res.IsValid);
        }

        [Fact]
        public void InvalidUserId_DoesNotPassValidation()
        {
            var request = new UpdateNotificationRequest
            {
                Id = 0
            };

            var res = _validator.Validate(request);

            Assert.False(res.IsValid);
        }
    }
}
