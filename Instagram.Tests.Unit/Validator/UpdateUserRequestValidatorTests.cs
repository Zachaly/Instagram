using Instagram.Application.Validation;
using Instagram.Models.User.Request;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Instagram.Tests.Unit.Validator
{
    public class UpdateUserRequestValidatorTests
    {
        private readonly UpdateUserRequestValidator _validator;

        public UpdateUserRequestValidatorTests()
        {
            _validator = new UpdateUserRequestValidator();
        }

        [Fact]
        public void ValidRequest_PassesValidation()
        {
            var request = new UpdateUserRequest
            {
                Id = 1,
            };

            var result = _validator.Validate(request);

            Assert.True(result.IsValid);
        }

        [Fact]
        public void InvalidId_DoesNotPassValidation()
        {
            var request = new UpdateUserRequest
            {
                Id = 0,
            };

            var result = _validator.Validate(request);

            Assert.False(result.IsValid);
        }

        [Theory]
        [InlineData(4)]
        [InlineData(51)]
        public void InvalidNameLength_DoesNotPassValidation(int length)
        {
            var request = new UpdateUserRequest
            {
                Id = 1,
                Name = new string('a', length)
            };

            var result = _validator.Validate(request);

            Assert.False(result.IsValid);
        }

        [Theory]
        [InlineData(4)]
        [InlineData(26)]
        public void InvalidNicknameLength_DoesNotPassValidation(int length)
        {
            var request = new UpdateUserRequest
            {
                Id = 1,
                Nickname = new string('a', length)
            };

            var result = _validator.Validate(request);

            Assert.False(result.IsValid);
        }

        [Fact]
        public void InvalidBioLength_DoesNotPassValidation()
        {
            var request = new UpdateUserRequest
            {
                Id = 1,
                Bio = new string('a', 201)
            };

            var result = _validator.Validate(request);

            Assert.False(result.IsValid);
        }
    }
}
