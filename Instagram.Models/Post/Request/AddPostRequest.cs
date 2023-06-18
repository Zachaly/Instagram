namespace Instagram.Models.Post.Request
{
    public class AddPostRequest
    {
        public long CreatorId { get; set; }
        public string Content { get; set; }
        public IEnumerable<string>? Tags { get; set; }
    }
}
