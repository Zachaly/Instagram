using Instagram.Application.Command;
using Instagram.Application.Validation;

namespace Instagram.Tests.Unit.Validator
{
    public class ResolvePostReportCommandValidatorTests
    {
        private readonly ResolvePostReportCommandValidator _validator;

        public ResolvePostReportCommandValidatorTests()
        {
            _validator = new ResolvePostReportCommandValidator();
        }

        [Fact]
        public void ValidCommand_PassesValidation()
        {
            var command = new ResolvePostReportCommand
            {
                Accepted = true,
                BanEndDate = DateTimeOffset.Now.AddDays(1).ToUnixTimeMilliseconds(),
                Id = 1,
                PostId = 2,
                UserId = 3,
            };

            var result = _validator.Validate(command);

            Assert.True(result.IsValid);
        }

        [Fact]
        public void InvalidBanEndDate_DoesNotPassValidation()
        {
            var command = new ResolvePostReportCommand
            {
                Accepted = true,
                BanEndDate = DateTimeOffset.Now.AddDays(-1).ToUnixTimeMilliseconds(),
                Id = 1,
                PostId = 2,
                UserId = 3,
            };

            var result = _validator.Validate(command);

            Assert.False(result.IsValid);
        }

        [Fact]
        public void InvalidId_DoesNotPassValidation()
        {
            var command = new ResolvePostReportCommand
            {
                Accepted = true,
                BanEndDate = DateTimeOffset.Now.AddDays(1).ToUnixTimeMilliseconds(),
                Id = 0,
                PostId = 2,
                UserId = 3,
            };

            var result = _validator.Validate(command);

            Assert.False(result.IsValid);
        }

        [Fact]
        public void InvalidPostId_DoesNotPassValidation()
        {
            var command = new ResolvePostReportCommand
            {
                Accepted = true,
                BanEndDate = DateTimeOffset.Now.AddDays(1).ToUnixTimeMilliseconds(),
                Id = 1,
                PostId = 0,
                UserId = 3,
            };

            var result = _validator.Validate(command);

            Assert.False(result.IsValid);
        }

        [Fact]
        public void InvalidUserId_DoesNotPassValidation()
        {
            var command = new ResolvePostReportCommand
            {
                Accepted = true,
                BanEndDate = DateTimeOffset.Now.AddDays(1).ToUnixTimeMilliseconds(),
                Id = 1,
                PostId = 2,
                UserId = 0,
            };

            var result = _validator.Validate(command);

            Assert.False(result.IsValid);
        }
    }
}
