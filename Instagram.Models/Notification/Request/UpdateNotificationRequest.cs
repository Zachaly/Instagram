namespace Instagram.Models.Notification.Request
{
    public class UpdateNotificationRequest
    {
        public long Id { get; set; }
        public bool? IsRead { get; set; }
    }
}
