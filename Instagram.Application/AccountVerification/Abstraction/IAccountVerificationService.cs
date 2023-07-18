using Instagram.Models.AccountVerification;
using Instagram.Models.AccountVerification.Request;

namespace Instagram.Application.Abstraction
{
    public interface IAccountVerificationService : IServiceBase<AccountVerificationModel, GetAccountVerificationRequest>
    {
    }
}
