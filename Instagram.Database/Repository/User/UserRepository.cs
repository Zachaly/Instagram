using Instagram.Database.Factory;
using Instagram.Database.Repository.Abstraction;
using Instagram.Database.Sql;
using Instagram.Domain.Entity;
using Instagram.Models.User;
using Instagram.Models.User.Request;

namespace Instagram.Database.Repository
{
    public class UserRepository : RepositoryBase<User, UserModel, GetUserRequest>, IUserRepository
    {
        public UserRepository(ISqlQueryBuilder sqlQueryBuilder, IConnectionFactory connectionFactory) : base(sqlQueryBuilder, connectionFactory)
        {
            Table = "User";
            DefaultOrderBy = "Id";
        }

        public Task<User> GetEntityByEmailAsync(string email)
        {
            var param = new { Email = email };
            var query = _sqlQueryBuilder
                .BuildSelect<User>(Table)
                .Where(param)
                .Build();

            return QuerySingleAsync<User>(query, param);
        }
    }
}
