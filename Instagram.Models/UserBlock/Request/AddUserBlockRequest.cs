namespace Instagram.Models.UserBlock.Request
{
    public class AddUserBlockRequest
    {
        public long BlockingUserId { get; set; }
        public long BlockedUserId { get; set; }
    }
}
