namespace Instagram.Models.PostImage
{
    public class PostImageModel : IModel
    {
        public long Id { get; set; }
        public long PostId { get; set; }
        public string File { get; set; }
    }
}
