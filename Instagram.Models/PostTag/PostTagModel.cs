namespace Instagram.Models.PostTag
{
    public class PostTagModel : IModel
    {
        public long PostId { get; set; }
        public string Tag { get; set; }
    }
}
