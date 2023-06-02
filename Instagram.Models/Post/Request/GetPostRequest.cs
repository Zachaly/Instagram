namespace Instagram.Models.Post.Request
{
    public class GetPostRequest : PagedRequest
    {
        public long? CreatorId { get; set; }
        public long? Id { get; set; }
    }
}
