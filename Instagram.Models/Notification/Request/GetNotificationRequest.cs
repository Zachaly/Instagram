namespace Instagram.Models.Notification.Request
{
    public class GetNotificationRequest : PagedRequest
    {
        public long? Id { get; set; }
        public long? UserId { get; set; }
        public string? Message { get; set; }
        public bool? IsRead { get; set; }
        public long? Created { get; set; }
    }
}
