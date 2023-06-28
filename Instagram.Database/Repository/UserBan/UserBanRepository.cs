using Instagram.Database.Factory;
using Instagram.Database.Repository.Abstraction;
using Instagram.Database.Sql;
using Instagram.Domain.Entity;
using Instagram.Models.UserBan;
using Instagram.Models.UserBan.Request;

namespace Instagram.Database.Repository
{
    public class UserBanRepository : RepositoryBase<UserBan, UserBanModel, GetUserBanRequest>, IUserBanRepository
    {
        public UserBanRepository(ISqlQueryBuilder sqlQueryBuilder, IConnectionFactory connectionFactory) : base(sqlQueryBuilder, connectionFactory)
        {
            Table = "UserBan";
            DefaultOrderBy = "[UserBan].[Id]";
        }
    }
}
