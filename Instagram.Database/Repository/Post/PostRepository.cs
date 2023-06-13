using Dapper;
using Instagram.Database.Factory;
using Instagram.Database.Repository.Abstraction;
using Instagram.Database.Sql;
using Instagram.Domain.Entity;
using Instagram.Models;
using Instagram.Models.Post;
using Instagram.Models.Post.Request;
using System.Linq;

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
                await connection.QueryAsync<PostModel, long, int, PostModel>(query, (post, imageId, likeCount) =>
                {
                    PostModel model;
                    if(!lookup.TryGetValue(post.Id, out model))
                    {
                        lookup.Add(post.Id, post);
                        model = post;
                        post.LikeCount = likeCount;
                    }

                    model.ImageIds ??= new List<long>();

                    if (!model.ImageIds.Contains(imageId))
                    {
                        (model.ImageIds as List<long>)!.Add(imageId);
                    }

                    return model;
                }, request, splitOn: "Id, Id, LikeCount");
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
                await connection.QueryAsync<PostModel, long, PostModel>(query, (post, imageId) =>
                {
                    model = model ?? post;

                    model.ImageIds ??= new List<long>();

                    if (!model.ImageIds.Contains(imageId))
                    {
                        (model.ImageIds as List<long>)!.Add(imageId);
                    }

                    return model;
                }, param);
            }

            return model;
        }
    }
}
