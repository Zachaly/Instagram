namespace Instagram.Models.PostTag.Request
{
    public class GetPostTagRequest : PagedRequest
    {
        public long? PostId { get; set; }
        public string? Tag { get; set; }
    }
}
