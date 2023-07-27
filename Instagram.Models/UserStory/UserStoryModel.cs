using Instagram.Domain.SqlAttribute;

namespace Instagram.Models.UserStory
{
    [Join(Condition = "[User].[Id]=[UserStoryImage].[UserId]", Table = "UserStoryImage", OutsideJoin = true)]
    public class UserStoryModel : IModel
    {
        [SqlName("[User].[Id]")]
        public long UserId { get; set; }

        [SqlName("[User].[Nickname]")]
        public string UserName { get; set; }

        [SqlName("[UserStoryImage].*")]
        public IEnumerable<UserStoryImageModel> Images { get; set; }
    }
}
