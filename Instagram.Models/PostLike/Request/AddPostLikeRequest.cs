namespace Instagram.Models.PostLike.Request
{
    public class AddPostLikeRequest
    {
        public long PostId { get; set; }
        public long UserId { get; set; }
    }
}
