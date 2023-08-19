using Instagram.Models.Notification;
using Instagram.Models.Notification.Request;
using Instagram.Models.Response;

namespace Instagram.Application.Abstraction
{
    public interface INotificationService : IServiceBase<NotificationModel, GetNotificationRequest, AddNotificationRequest>
    {
        Task<ResponseModel> DeleteByIdAsync(long id);
        Task<ResponseModel> UpdateAsync(UpdateNotificationRequest request);
    }
}
