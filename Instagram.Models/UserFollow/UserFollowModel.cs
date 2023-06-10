using Instagram.Domain.SqlAttribute;

namespace Instagram.Models.UserFollow
{
    public class UserFollowModel : IModel
    {
        public long FollowingUserId { get; set; }
        public long FollowedUserId { get; set; }
        [SqlName(Conditional = true)]
        public string? UserName { get; set; }
    }
}
