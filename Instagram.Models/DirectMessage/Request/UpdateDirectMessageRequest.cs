namespace Instagram.Models.DirectMessage.Request
{
    public class UpdateDirectMessageRequest
    {
        public long Id { get; set; }
        public string? Content { get; set; }
        public bool? Read { get; set; }
    }
}
