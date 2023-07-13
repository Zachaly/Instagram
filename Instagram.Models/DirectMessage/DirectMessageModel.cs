namespace Instagram.Models.DirectMessage
{
    public class DirectMessageModel : IModel
    {
        public long Id { get; set; }
        public long SenderId { get; set; }
        public bool Read { get; set; }
        public long Created { get; set; }
        public string Content { get; set; }
    }
}
