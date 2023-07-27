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

        public Task<IEnumerable<UserStoryModel>> GetStoriesAsync(GetUserStoryRequest request)
        {
            throw new NotImplementedException();
        }
    }
}
