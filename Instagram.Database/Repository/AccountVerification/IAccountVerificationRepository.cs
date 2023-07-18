using Instagram.Database.Repository.Abstraction;
using Instagram.Domain.Entity;
using Instagram.Models.AccountVerification;
using Instagram.Models.AccountVerification.Request;

namespace Instagram.Database.Repository
{
    public interface IAccountVerificationRepository : IRepositoryBase<AccountVerification, AccountVerificationModel, GetAccountVerificationRequest>
    {
    }
}
