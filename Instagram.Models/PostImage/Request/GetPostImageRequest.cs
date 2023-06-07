namespace Instagram.Models.PostImage.Request
{
    public class GetPostImageRequest : PagedRequest
    {
        public long? Id { get; set; }
        public long? PostId { get; set; }
    }
}
