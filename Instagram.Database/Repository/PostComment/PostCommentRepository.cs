using Instagram.Database.Factory;
using Instagram.Database.Repository.Abstraction;
using Instagram.Database.Sql;
using Instagram.Domain.Entity;
using Instagram.Models.PostComment;
using Instagram.Models.PostComment.Request;

namespace Instagram.Database.Repository
{
    internal class PostCommentRepository : RepositoryBase<PostComment, PostCommentModel, GetPostCommentRequest>, IPostCommentRepository
    {
        public PostCommentRepository(ISqlQueryBuilder sqlQueryBuilder, IConnectionFactory connectionFactory) : base(sqlQueryBuilder, connectionFactory)
        {
            Table = "PostComment";
            DefaultOrderBy = "[PostComment].[Created] ASC";
        }
    }
}
