using Instagram.Database.Repository.Abstraction;
using Instagram.Domain.Entity;
using Instagram.Models.Notification;
using Instagram.Models.Notification.Request;

namespace Instagram.Database.Repository
{
    public interface INotificationRepository : IRepositoryBase<Notification, NotificationModel, GetNotificationRequest>
    {
        Task UpdateAsync(UpdateNotificationRequest request);
    }
}
