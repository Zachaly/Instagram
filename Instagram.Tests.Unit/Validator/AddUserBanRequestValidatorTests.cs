using Instagram.Application.Validation;
using Instagram.Models.UserBan.Request;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Instagram.Tests.Unit.Validator
{
    public class AddUserBanRequestValidatorTests
    {
        private readonly AddUserBanRequestValidator _validator;

        public AddUserBanRequestValidatorTests()
        {
            _validator = new AddUserBanRequestValidator();
        }

        [Fact]
        public void ValidRequest_DoesNotPassValidation()
        {
            var request = new AddUserBanRequest
            {
                EndDate = DateTimeOffset.UtcNow.AddDays(1).ToUnixTimeMilliseconds(),
                UserId = 1,
            };

            var result = _validator.Validate(request);

            Assert.True(result.IsValid);
        }

        [Fact]
        public void InvalidEndDate_DoesNotPassValidation()
        {
            var request = new AddUserBanRequest
            {
                EndDate = DateTimeOffset.UtcNow.AddDays(-1).ToUnixTimeMilliseconds(),
                UserId = 1,
            };

            var result = _validator.Validate(request);

            Assert.False(result.IsValid);
        }

        [Fact]
        public void InvalidUserId_DoesNotPassValidation()
        {
            var request = new AddUserBanRequest
            {
                EndDate = DateTimeOffset.UtcNow.AddDays(1).ToUnixTimeMilliseconds(),
                UserId = 0,
            };

            var result = _validator.Validate(request);

            Assert.False(result.IsValid);
        }
    }
}
