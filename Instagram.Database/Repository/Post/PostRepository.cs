using Dapper;
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
            DefaultOrderBy = "[Post].[Created] DESC";
        }

        public override async Task<IEnumerable<PostModel>> GetAsync(GetPostRequest request)
        {
            var query = _sqlQueryBuilder
                .BuildSelect<PostModel>(Table)
                .Where(request)
                .WithPagination(request)
                .OrderBy(DefaultOrderBy)
                .Build();

            var lookup = new Dictionary<long, PostModel>();

            using(var connection = _connectionFactory.CreateConnection())
            {
                await connection.QueryAsync<PostModel, long, int, int, string, PostModel>(query, (post, imageId, likeCount, commentCount, tag) =>
                {
                    PostModel model;
                    if(!lookup.TryGetValue(post.Id, out model))
                    {
                        lookup.Add(post.Id, post);
                        model = post;
                        post.LikeCount = likeCount;
                        post.CommentCount = commentCount;
                    }

                    model.ImageIds ??= new List<long>();

                    model.Tags ??= new List<string>();

                    if (!model.ImageIds.Contains(imageId))
                    {
                        (model.ImageIds as List<long>)!.Add(imageId);
                    }

                    if (!model.Tags.Contains(tag) && tag is not null)
                    {
                        (model.Tags as List<string>)!.Add(tag);
                    }

                    return model;
                }, request, splitOn: "Id, Id, LikeCount, CommentCount, Tag");
            }

            return lookup.Values;
        }

        public override async Task<PostModel> GetByIdAsync(long id)
        {
            var param = new { Id = id };
            var query = _sqlQueryBuilder
                .BuildSelect<PostModel>(Table)
                .Where(param)
                .Build();

            PostModel model = null;
            using (var connection = _connectionFactory.CreateConnection())
            {
                await connection.QueryAsync<PostModel, long, int, int, string, PostModel>(query, (post, imageId, likeCount, commentCount, tag) =>
                {
                    model = model ?? post;

                    model.LikeCount = likeCount;
                    model.CommentCount = commentCount;

                    model.ImageIds ??= new List<long>();

                    model.Tags ??= new List<string>();

                    if (!model.ImageIds.Contains(imageId))
                    {
                        (model.ImageIds as List<long>)!.Add(imageId);
                    }

                    if (!model.Tags.Contains(tag) && tag is not null)
                    {
                        (model.Tags as List<string>)!.Add(tag);
                    }

                    return model;
                }, param, splitOn: "Id, Id, LikeCount, CommentCount, Tag");
            }

            return model;
        }
    }
}
