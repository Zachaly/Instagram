using Instagram.Application.Abstraction;
using Instagram.Domain.Entity;
using Instagram.Models.AccountVerification.Request;

namespace Instagram.Application
{
    public class AccountVerificationFactory : IAccountVerificationFactory
    {
        public AccountVerification Create(AddAccountVerificationRequest request, string documentFile)
            => new AccountVerification
            {
                Created = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds(),
                DateOfBirth = request.DateOfBirth,
                DocumentFileName = documentFile,
                FirstName = request.FirstName,
                LastName = request.LastName,
                UserId = request.UserId,
            };
    }
}
