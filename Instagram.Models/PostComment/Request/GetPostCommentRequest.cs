namespace Instagram.Models.PostComment.Request
{
    public class GetPostCommentRequest : PagedRequest
    {
        public long? Id { get; set; }
        public long? UserId { get; set; }
        public long? PostId { get; set; }
        public string? Content { get; set; }
        public long? Created { get; set; }
    }
}
