namespace Instagram.Models.PostTag.Request
{
    public class AddPostTagRequest
    {
        public long PostId { get; set; }
        public IEnumerable<string> Tags { get; set; }
    }
}
