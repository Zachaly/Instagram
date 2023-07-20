using Instagram.Application.Abstraction;
using Instagram.Database.Repository;
using Instagram.Domain.Entity;
using Instagram.Models.Notification;
using Instagram.Models.Notification.Request;
using Instagram.Models.Response;

namespace Instagram.Application
{
    internal class NotificationService : ServiceBase<Notification, NotificationModel, GetNotificationRequest, INotificationRepository>,
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

        public Task<ResponseModel> AddAsync(AddNotificationRequest request)
        {
            throw new NotImplementedException();
        }

        public Task<ResponseModel> DeleteByIdAsync(long id)
        {
            throw new NotImplementedException();
        }
    }
}
