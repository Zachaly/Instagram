using Instagram.Models.VerificationRequest;
using Instagram.Models.VerificationRequest.Request;

namespace Instagram.Application.Abstraction
{
    public interface IAccountVerificationService : IServiceBase<AccountVerificationModel, GetAccountVerificationRequest>
    {
    }
}
