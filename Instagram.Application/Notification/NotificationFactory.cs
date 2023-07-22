using Instagram.Application.Abstraction;
using Instagram.Domain.Entity;
using Instagram.Models.Notification.Request;

namespace Instagram.Application
{
    public class NotificationFactory : INotificationFactory
    {
        public Notification Create(AddNotificationRequest request)
            => new Notification
            {
                Created = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds(),
                IsRead = false,
                Message = request.Message,
                UserId = request.UserId,
            };
    }
}
