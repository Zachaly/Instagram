using Instagram.Application.Abstraction;
using Instagram.Database.Repository;
using Instagram.Domain.Entity;
using Instagram.Models.AccountVerification;
using Instagram.Models.AccountVerification.Request;

namespace Instagram.Application
{
    public class AccountVerificationService : ServiceBase<AccountVerification, AccountVerificationModel, GetAccountVerificationRequest, IAccountVerificationRepository>, IAccountVerificationService
    {
        public AccountVerificationService(IAccountVerificationRepository repository) : base(repository)
        {
        }
    }
}
