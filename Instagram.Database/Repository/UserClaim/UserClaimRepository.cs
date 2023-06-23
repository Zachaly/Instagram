using Instagram.Database.Factory;
using Instagram.Database.Repository.Abstraction;
using Instagram.Database.Sql;
using Instagram.Domain.Entity;
using Instagram.Models.UserClaim;
using Instagram.Models.UserClaim.Request;

namespace Instagram.Database.Repository
{
    public class UserClaimRepository : KeylessRepositoryBase<UserClaim, UserClaimModel, GetUserClaimRequest>, IUserClaimRepository
    {
        public UserClaimRepository(ISqlQueryBuilder sqlQueryBuilder, IConnectionFactory connectionFactory) : base(sqlQueryBuilder, connectionFactory)
        {
            Table = "UserClaim";
            DefaultOrderBy = "[UserClaim].[Value]";
        }

        public Task DeleteAsync(long userId, string value)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<UserClaim>> GetEntitiesAsync(GetUserClaimRequest request)
        {
            throw new NotImplementedException();
        }
    }
}
