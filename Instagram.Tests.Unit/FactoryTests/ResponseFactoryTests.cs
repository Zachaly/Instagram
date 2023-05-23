using FluentValidation.Results;
using Instagram.Application;

namespace Instagram.Tests.Unit.FactoryTests
{
    public class ResponseFactoryTests
    {
        private readonly ResponseFactory _factory;

        public ResponseFactoryTests()
        {
            _factory = new ResponseFactory();
        }

        [Fact]
        public void Create_Success_CreatesValidSuccessResponse()
        {
            var res = _factory.CreateSuccess();

            Assert.True(res.Success);
        }

        [Fact]
        public void CreateSuccess_CreatedValidSuccessDataResponse()
        {
            const long Data = 1;
            var res = _factory.CreateSuccess<long>(Data);

            Assert.True(res.Success);
            Assert.Equal(Data, res.Data);
        }

        [Fact]
        public void CreateSuccess_WithEntityId_CreatesValidSuccessResponse()
        {
            const long Id = 1;
            var res = _factory.CreateSuccess(Id);

            Assert.True(res.Success);
            Assert.Equal(Id, res.NewEntityId);
        }

        [Fact]
        public void CreateFailure_CreatesValidFailureResponse()
        {
            const string Error = "err";
            var res = _factory.CreateFailure(Error);

            Assert.False(res.Success);
            Assert.Equal(Error, res.Error);
        }

        [Fact]
        public void CreateFailure_CreatesValidFailureDataResponse()
        {
            const string Error = "err";
            var res = _factory.CreateFailure<object>(Error);

            Assert.False(res.Success);
            Assert.Equal(Error, res.Error);
            Assert.Equal(default, res.Data);
        }

        [Fact]
        public void CreateValidationError_CreatesValidFailureResponse()
        {
            var failures = new List<ValidationFailure>
            {
                new ValidationFailure("prop1", "message1"),
                new ValidationFailure("prop1", "message2"),
                new ValidationFailure("prop2", "message3")
            };
            var validationResult = new ValidationResult(failures);

            var res = _factory.CreateValidationError(validationResult);

            Assert.False(res.Success);
            Assert.Equivalent(res.ValidationErrors!.Keys, failures.Select(x => x.PropertyName));
            Assert.Equivalent(res.ValidationErrors!.Values, failures.Select(x => x.ErrorMessage));
        }
    }
}
