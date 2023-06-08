using Instagram.Domain.SqlAttribute;

namespace Instagram.Models.UserFollow
{
    public class UserFollowModel : IModel
    {
        public long FollowingUserId { get; set; }
        public long FollowedUserId { get; set; }
        [SqlName("[User].[Name] as UserName")]
        public long UserName { get; set; }
    }
}
