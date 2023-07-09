using Instagram.Application.Validation;
using Instagram.Models.PostReport.Request;

namespace Instagram.Tests.Unit.Validator
{
    public class AddPostReportRequestValidatorTests
    {
        private readonly AddPostReportRequestValidator _validator;

        public AddPostReportRequestValidatorTests()
        {
            _validator = new AddPostReportRequestValidator();
        }

        [Fact]
        public void ValidRequest_PassesValidation()
        {
            var request = new AddPostReportRequest
            {
                PostId = 1,
                Reason = "min 10 letter reason",
                ReportingUserId = 1,
            };

            var result = _validator.Validate(request);

            Assert.True(result.IsValid);
        }

        [Fact]
        public void InvalidPostId_DoesNotPassValidation()
        {
            var request = new AddPostReportRequest
            {
                PostId = 0,
                Reason = "min 10 letter reason",
                ReportingUserId = 1,
            };

            var result = _validator.Validate(request);

            Assert.False(result.IsValid);
        }

        [Fact]
        public void InvalidReportingUserId_DoesNotPassValidation()
        {
            var request = new AddPostReportRequest
            {
                PostId = 1,
                Reason = "min 10 letter reason",
                ReportingUserId = 0,
            };

            var result = _validator.Validate(request);

            Assert.False(result.IsValid);
        }

        [Theory]
        [InlineData(9)]
        [InlineData(201)]
        public void InvalidReasonLength_DoesNotPassValidation(int length)
        {
            var request = new AddPostReportRequest
            {
                PostId = 1,
                Reason = new string('a', length),
                ReportingUserId = 1,
            };

            var result = _validator.Validate(request);

            Assert.False(result.IsValid);
        }
    }
}
