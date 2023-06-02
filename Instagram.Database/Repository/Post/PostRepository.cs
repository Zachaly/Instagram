using Instagram.Database.Factory;
using Instagram.Database.Repository.Abstraction;
using Instagram.Database.Sql;
using Instagram.Domain.Entity;
using Instagram.Models.Post;
using Instagram.Models.Post.Request;

namespace Instagram.Database.Repository
{
    public class PostRepository : RepositoryBase<Post, PostModel, GetPostRequest>, IPostRepository
    {
        public PostRepository(ISqlQueryBuilder sqlQueryBuilder, IConnectionFactory connectionFactory) : base(sqlQueryBuilder, connectionFactory)
        {
            Table = "Post";
            DefaultOrderBy = "Created";
        }
    }
}
