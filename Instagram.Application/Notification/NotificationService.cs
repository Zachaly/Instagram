using Instagram.Application.Abstraction;
using Instagram.Database.Repository;
using Instagram.Domain.Entity;
using Instagram.Models.Notification;
using Instagram.Models.Notification.Request;
using Instagram.Models.Response;

namespace Instagram.Application
{
    public class NotificationService : ServiceBase<Notification, NotificationModel, GetNotificationRequest, AddNotificationRequest, INotificationRepository>,
        INotificationService
    {
        public NotificationService(INotificationRepository repository, INotificationFactory notificationFactory,
            IResponseFactory responseFactory) : base(repository, notificationFactory, responseFactory)
        {
        }

        public async Task<ResponseModel> DeleteByIdAsync(long id)
        {
            try
            {
                await _repository.DeleteByIdAsync(id);

                return _responseFactory.CreateSuccess();
            }
            catch(Exception ex)
            {
                return _responseFactory.CreateFailure(ex.Message);
            }
        }

        public async Task<ResponseModel> UpdateAsync(UpdateNotificationRequest request)
        {
            try
            {
                await _repository.UpdateAsync(request);

                return _responseFactory.CreateSuccess();
            }
            catch (Exception ex)
            {
                return _responseFactory.CreateFailure(ex.Message);
            }
        }
    }
}
