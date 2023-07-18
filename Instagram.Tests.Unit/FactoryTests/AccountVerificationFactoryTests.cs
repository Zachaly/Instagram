using Instagram.Application;
using Instagram.Models.AccountVerification.Request;

namespace Instagram.Tests.Unit.FactoryTests
{
    public class AccountVerificationFactoryTests
    {
        private readonly AccountVerificationFactory _factory;

        public AccountVerificationFactoryTests()
        {
            _factory = new AccountVerificationFactory();
        }

        [Fact]
        public void Create_CreatesValidEntity()
        {
            var request = new AddAccountVerificationRequest
            {
                DateOfBirth = "date",
                FirstName = "fname",
                LastName = "lname",
                UserId = 1
            };

            const string DocumentFileName = "document";

            var verification = _factory.Create(request, DocumentFileName);

            Assert.Equal(DocumentFileName, verification.DocumentFileName);
            Assert.Equal(request.FirstName, verification.FirstName);
            Assert.Equal(request.LastName, verification.LastName);
            Assert.Equal(request.DateOfBirth, verification.DateOfBirth);
            Assert.Equal(request.UserId, verification.UserId);
            Assert.NotEqual(default, verification.Created);
        }
    }
}
