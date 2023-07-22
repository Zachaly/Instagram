using Instagram.Application.Abstraction;
using Instagram.Database.Repository;
using Instagram.Domain.Entity;
using Instagram.Models.Notification;
using Instagram.Models.Notification.Request;
using Instagram.Models.Response;

namespace Instagram.Application
{
    public class NotificationService : ServiceBase<Notification, NotificationModel, GetNotificationRequest, INotificationRepository>,
        INotificationService
    {
        private readonly INotificationFactory _notificationFactory;
        private readonly IResponseFactory _responseFactory;

        public NotificationService(INotificationRepository repository, INotificationFactory notificationFactory,
            IResponseFactory responseFactory) : base(repository)
        {
            _notificationFactory = notificationFactory;
            _responseFactory = responseFactory;
        }

        public async Task<ResponseModel> AddAsync(AddNotificationRequest request)
        {
            try
            {
                var notification = _notificationFactory.Create(request);

                var id = await _repository.InsertAsync(notification);

                return _responseFactory.CreateSuccess(id);
            }
            catch (Exception ex)
            {
                return _responseFactory.CreateFailure(ex.Message);
            }
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
