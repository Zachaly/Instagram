namespace Instagram.Models.PostComment.Request
{
    public class AddPostCommentRequest
    {
        public long UserId { get; set; }
        public long PostId { get; set; }
        public string Content { get; set; }
    }
}
