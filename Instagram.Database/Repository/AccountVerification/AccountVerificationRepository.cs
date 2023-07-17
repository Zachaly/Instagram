using Instagram.Database.Factory;
using Instagram.Database.Repository.Abstraction;
using Instagram.Database.Sql;
using Instagram.Domain.Entity;
using Instagram.Models.VerificationRequest;
using Instagram.Models.VerificationRequest.Request;

namespace Instagram.Database.Repository
{
    public class AccountVerificationRepository : RepositoryBase<AccountVerification, AccountVerificationModel, GetAccountVerificationRequest>, IAccountVerificationRepository
    {
        public AccountVerificationRepository(ISqlQueryBuilder sqlQueryBuilder, IConnectionFactory connectionFactory) : base(sqlQueryBuilder, connectionFactory)
        {
            Table = "AccountVerification";
            DefaultOrderBy = "[AccountVerification].[Id]";
        }
    }
}
