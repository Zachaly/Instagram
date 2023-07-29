using Dapper;
using Instagram.Database.Factory;
using Instagram.Database.Repository.Abstraction;
using Instagram.Database.Sql;
using Instagram.Domain.Entity;
using Instagram.Models.UserStory;
using Instagram.Models.UserStory.Request;

namespace Instagram.Database.Repository
{
    public class UserStoryImageRepository : RepositoryBase<UserStoryImage, UserStoryImageModel, GetUserStoryRequest>, IUserStoryImageRepository
    {
        public UserStoryImageRepository(ISqlQueryBuilder sqlQueryBuilder, IConnectionFactory connectionFactory) : base(sqlQueryBuilder, connectionFactory)
        {
            Table = "UserStoryImage";
            DefaultOrderBy = "[UserStoryImage].[Created] DESC";
        }

        public async Task<IEnumerable<UserStoryModel>> GetStoriesAsync(GetUserStoryRequest request)
        {
            var query = _sqlQueryBuilder
                .BuildSelect<UserStoryModel>("User")
                .Where(request)
                .WithPagination(request)
                .OrderBy("[User].[Id]")
                .OuterOrderBy("[UserStoryImage].[Created]")
                .Build();

            var lookup = new Dictionary<long, UserStoryModel>();

            using(var connection = _connectionFactory.CreateConnection())
            {
                await connection.QueryAsync<UserStoryModel, UserStoryImage, UserStoryModel>(query, (story, image) =>
                {

                    UserStoryModel? model;

                    if(!lookup.TryGetValue(story.UserId, out model))
                    {
                        model = story;
                        lookup.Add(story.UserId, model);
                    }

                    model.Images ??= new List<UserStoryImageModel>();

                    if(model.UserId == image?.UserId)
                    {
                        (model.Images as List<UserStoryImageModel>)!.Add(new UserStoryImageModel 
                        {
                            Created = image.Created,
                            Id = image.Id
                        });
                    }

                    return model;
                }, request, splitOn: "Id, Id");
            }

            return lookup.Values;
        }
    }
}
