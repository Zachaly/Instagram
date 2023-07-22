using Instagram.Domain.Entity;
using Instagram.Models.Notification.Request;

namespace Instagram.Application.Abstraction
{
    public interface INotificationFactory
    {
        Notification Create(AddNotificationRequest request);
    }
}
