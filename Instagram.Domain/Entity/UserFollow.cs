namespace Instagram.Domain.Entity
{
    public class UserFollow : IEntity
    {
        public long FollowingUserId { get; set; }
        public long FollowedUserId { get; set; }
    }
}
