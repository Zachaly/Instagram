using Instagram.Database.Factory;
using Instagram.Database.Repository.Abstraction;
using Instagram.Database.Sql;
using Instagram.Domain.Entity;
using Instagram.Models.PostLike;
using Instagram.Models.PostLike.Request;

namespace Instagram.Database.Repository
{
    public class PostLikeRepository : KeylessRepositoryBase<PostLike, PostLikeModel, GetPostLikeRequest>, IPostLikeRepository
    {
        public PostLikeRepository(ISqlQueryBuilder sqlQueryBuilder, IConnectionFactory connectionFactory) : base(sqlQueryBuilder, connectionFactory)
        {
            Table = "PostLike";
            DefaultOrderBy = "[PostLike].[UserId]";
        }

        public Task DeleteAsync(long postId, long userId)
        {
            throw new NotImplementedException();
        }
    }
}
