using Instagram.Domain.SqlAttribute;

namespace Instagram.Models.UserFollow.Request
{
    public class GetUserFollowRequest : PagedRequest
    {
        public long? FollowingUserId { get; set; }
        public long? FollowedUserId { get; set; }
        [ConditionalJoin(Table = "User", Condition = "[User].[Id]=[UserFollow].[FollowingUserId]", ExclusiveWith = "JoinFollowed")]
        public bool? JoinFollower { get; set; }
        [ConditionalJoin(Table = "User", Condition = "[User].[Id]=[UserFollow].[FollowedUserId]", ExclusiveWith = "JoinFollower")]
        public bool? JoinFollowed { get; set; }
    }
}
