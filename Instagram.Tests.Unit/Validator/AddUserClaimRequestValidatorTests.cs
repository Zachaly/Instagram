using Instagram.Application.Validation;
using Instagram.Models.UserClaim.Request;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Instagram.Tests.Unit.Validator
{
    public class AddUserClaimRequestValidatorTests
    {
        private readonly AddUserClaimRequestValidator _validator;

        public AddUserClaimRequestValidatorTests()
        {
            _validator = new AddUserClaimRequestValidator();
        }

        [Fact]
        public void ValidRequest_PassesValidation()
        {
            var request = new AddUserClaimRequest
            {
                UserId = 1,
                Value = "claim"
            };

            var result = _validator.Validate(request);

            Assert.True(result.IsValid);
        }

        [Fact]
        public void InvalidUserId_DoesNotPassValidation()
        {
            var request = new AddUserClaimRequest
            {
                UserId = 0,
                Value = "claim"
            };

            var result = _validator.Validate(request);

            Assert.False(result.IsValid);
        }

        [Theory]
        [InlineData(0)]
        [InlineData(31)]
        public void InvalidValueLength_DoesNotPassValidation(int length)
        {
            var request = new AddUserClaimRequest
            {
                UserId = 1,
                Value = new string('a', length)
            };

            var result = _validator.Validate(request);

            Assert.False(result.IsValid);
        }
    }
}
