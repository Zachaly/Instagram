using Instagram.Database.Factory;
using Instagram.Database.Repository.Abstraction;
using Instagram.Database.Sql;
using Instagram.Domain.Entity;
using Instagram.Models.PostImage;
using Instagram.Models.PostImage.Request;

namespace Instagram.Database.Repository
{
    public class PostImageRepository : RepositoryBase<PostImage, PostImageModel, GetPostImageRequest>, IPostImageRepository
    {
        public PostImageRepository(ISqlQueryBuilder sqlQueryBuilder, IConnectionFactory connectionFactory) : base(sqlQueryBuilder, connectionFactory)
        {
            Table = "PostImage";
            DefaultOrderBy = "[PostImage].[Id]";
        }

        public Task DeleteByPostIdAsync(long postId)
        {
            throw new NotImplementedException();
        }
    }
}
