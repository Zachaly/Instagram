using Instagram.Domain.Entity;
using Instagram.Models.VerificationRequest.Request;

namespace Instagram.Application.Abstraction
{
    public interface IAccountVerificationFactory
    {
        AccountVerification Create(AddAccountVerificationRequest request, string documentFile);
    }
}
