using Instagram.Application.Abstraction;
using Instagram.Domain.Entity;
using Instagram.Models.Notification.Request;

namespace Instagram.Application
{
    public class NotificationFactory : INotificationFactory
    {
        public Notification Create(AddNotificationRequest request)
        {
            throw new NotImplementedException();
        }
    }
}
