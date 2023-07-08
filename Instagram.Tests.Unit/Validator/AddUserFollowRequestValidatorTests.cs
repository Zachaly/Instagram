using Instagram.Application.Validation;
using Instagram.Models.UserFollow.Request;

namespace Instagram.Tests.Unit.Validator
{
    public class AddUserFollowRequestValidatorTests
    {
        private readonly AddUserFollowRequestValidator _validator;

        public AddUserFollowRequestValidatorTests()
        {
            _validator = new AddUserFollowRequestValidator();
        }

        [Fact]
        public void ValidRequest_PassesValidation()
        {
            var request = new AddUserFollowRequest
            {
                FollowedUserId = 1,
                FollowingUserId = 2,
            };

            var result = _validator.Validate(request);

            Assert.True(result.IsValid);
        }

        [Fact]
        public void InvalidFollowingUserId_DoesNotPassValidation()
        {
            var request = new AddUserFollowRequest
            {
                FollowedUserId = 1,
                FollowingUserId = 0,
            };

            var result = _validator.Validate(request);

            Assert.False(result.IsValid);
        }

        [Fact]
        public void InvalidFollowedUserId_DoesNotPassValidation()
        {
            var request = new AddUserFollowRequest
            {
                FollowedUserId = 0,
                FollowingUserId = 1,
            };

            var result = _validator.Validate(request);

            Assert.False(result.IsValid);
        }
    }
}
