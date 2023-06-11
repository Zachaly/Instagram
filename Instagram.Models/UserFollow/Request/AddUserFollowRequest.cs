namespace Instagram.Models.UserFollow.Request
{
    public class AddUserFollowRequest
    {
        public long FollowingUserId { get; set; }
        public long FollowedUserId { get; set; }
    }
}
