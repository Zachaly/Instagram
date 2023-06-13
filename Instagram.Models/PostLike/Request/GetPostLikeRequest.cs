namespace Instagram.Models.PostLike.Request
{
    public class GetPostLikeRequest : PagedRequest
    {
        public long? PostId { get; set; }
        public long? UserId { get; set; }
    }
}
