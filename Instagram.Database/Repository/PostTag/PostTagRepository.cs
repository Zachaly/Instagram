using Instagram.Database.Factory;
using Instagram.Database.Repository.Abstraction;
using Instagram.Database.Sql;
using Instagram.Domain.Entity;
using Instagram.Models.PostTag;
using Instagram.Models.PostTag.Request;

namespace Instagram.Database.Repository
{
    public class PostTagRepository : KeylessRepositoryBase<PostTag, PostTagModel, GetPostTagRequest>, IPostTagRepository
    {
        public PostTagRepository(ISqlQueryBuilder sqlQueryBuilder, IConnectionFactory connectionFactory) : base(sqlQueryBuilder, connectionFactory)
        {
            Table = "PostTag";
            DefaultOrderBy = "[PostTag].[PostId]";
        }

        public Task DeleteAsync(long postId, string tag)
        {
            var param = new { PostId = postId, Tag = tag };

            var query = _sqlQueryBuilder
                .BuildDelete(Table)
                .Where(param)
                .Build();

            return QueryAsync(query, param);
        }

        public Task DeleteByPostIdAsync(long postId)
        {
            var param = new { PostId = postId };

            var query = _sqlQueryBuilder
                .BuildDelete(Table)
                .Where(param)
                .Build();

            return QueryAsync(query, param);
        }
    }
}
