using Instagram.Application.Validation;
using Instagram.Models.UserBlock.Request;

namespace Instagram.Tests.Unit.Validator
{
    public class AddUserBlockRequestValidatorTests
    {
        private readonly AddUserBlockRequestValidator _validator;

        public AddUserBlockRequestValidatorTests()
        {
            _validator = new AddUserBlockRequestValidator();
        }

        [Fact]
        public void ValidRequest_PassesValidation()
        {
            var request = new AddUserBlockRequest
            {
                BlockedUserId = 1,
                BlockingUserId = 2
            };

            var res = _validator.Validate(request);

            Assert.True(res.IsValid);
        }

        [Fact]
        public void InvalidBlockedUserId_DoesNotPassValidation()
        {
            var request = new AddUserBlockRequest
            {
                BlockedUserId = 0,
                BlockingUserId = 1
            };

            var res = _validator.Validate(request);

            Assert.False(res.IsValid);
        }

        [Fact]
        public void InvalidBlockingUserId_DoesNotPassValidation()
        {
            var request = new AddUserBlockRequest
            {
                BlockedUserId = 1,
                BlockingUserId = 0
            };

            var res = _validator.Validate(request);

            Assert.False(res.IsValid);
        }
    }
}
