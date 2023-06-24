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
            var param = new { UserId = userId, Value = value };

            var query = _sqlQueryBuilder
                .BuildDelete(Table)
                .Where(param)
                .Build();

            return QueryAsync(query, param);
        }

        public Task<IEnumerable<UserClaim>> GetEntitiesAsync(GetUserClaimRequest request)
        {
            var query = _sqlQueryBuilder
                .BuildSelect<UserClaim>(Table)
                .Where(request)
                .WithPagination(request)
                .OrderBy(DefaultOrderBy)
                .Build();

            return QueryManyAsync<UserClaim>(query, request);
        }
    }
}
