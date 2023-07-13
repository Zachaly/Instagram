namespace Instagram.Models.DirectMessage.Request
{
    public class AddDirectMessageRequest
    {
        public long SenderId { get; set; }
        public long ReceiverId { get; set; }
        public string Content { get; set; }
    }
}
