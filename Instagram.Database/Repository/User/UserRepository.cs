using Instagram.Database.Factory;
using Instagram.Database.Repository.Abstraction;
using Instagram.Database.Sql;
using Instagram.Domain.Entity;
using Instagram.Models.User;
using Instagram.Models.User.Request;

namespace Instagram.Database.Repository
{
    internal class UserRepository : RepositoryBase<User, UserModel, GetUserRequest>, IUserRepository
    {
        public UserRepository(ISqlQueryBuilder sqlQueryBuilder, IConnectionFactory connectionFactory) : base(sqlQueryBuilder, connectionFactory)
        {
            Table = "User";
            DefaultOrderBy = "Id";
        }

        public Task<User> GetEntityByLoginAsync(string login)
        {
            throw new NotImplementedException();
        }
    }
}
