using Instagram.Application.Abstraction;
using Instagram.Domain.Entity;
using Instagram.Models.AccountVerification.Request;

namespace Instagram.Application
{
    public class AccountVerificationFactory : IAccountVerificationFactory
    {
        public AccountVerification Create(AddAccountVerificationRequest request, string documentFile)
        {
            throw new NotImplementedException();
        }
    }
}
