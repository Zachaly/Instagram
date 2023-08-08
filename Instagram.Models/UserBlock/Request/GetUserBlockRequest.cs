namespace Instagram.Models.UserBlock.Request
{
    public class GetUserBlockRequest : PagedRequest
    {
        public long? Id { get; set; }
        public long? BlockedUserId { get; set; }
        public long? BlockingUserId { get; set; }
    }
}
