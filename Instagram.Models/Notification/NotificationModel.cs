namespace Instagram.Models.Notification
{
    public class NotificationModel : IModel
    {
        public long Id { get; set; }
        public string Message { get; set; }
        public long Created { get; set; }
        public bool IsRead { get; set; }
    }
}
