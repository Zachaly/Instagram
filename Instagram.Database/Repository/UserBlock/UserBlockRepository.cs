using Instagram.Database.Factory;
using Instagram.Database.Repository.Abstraction;
using Instagram.Database.Sql;
using Instagram.Domain.Entity;
using Instagram.Models.UserBlock;
using Instagram.Models.UserBlock.Request;

namespace Instagram.Database.Repository
{
    public class UserBlockRepository : RepositoryBase<UserBlock, UserBlockModel, GetUserBlockRequest>, IUserBlockRepository
    {
        public UserBlockRepository(ISqlQueryBuilder sqlQueryBuilder, IConnectionFactory connectionFactory) : base(sqlQueryBuilder, connectionFactory)
        {
            Table = "UserBlock";
            DefaultOrderBy = "[UserBlock].[Id]";
        }
    }
}
