using Instagram.Application;
using Instagram.Application.Abstraction;
using Instagram.Database.Repository;
using Instagram.Domain.Entity;
using Instagram.Models.Notification.Request;
using Instagram.Models.Response;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Instagram.Tests.Unit.ServiceTests
{
    public class NotificationServiceTests
    {
        private readonly Mock<INotificationRepository> _notificationRepository;
        private readonly Mock<INotificationFactory> _notificationFactory;
        private readonly Mock<IResponseFactory> _responseFactory;
        private readonly NotificationService _service;

        public NotificationServiceTests()
        {
            _notificationRepository = new Mock<INotificationRepository>();
            _notificationFactory = new Mock<INotificationFactory>();
            _responseFactory = new Mock<IResponseFactory>();

            _responseFactory.Setup(x => x.CreateSuccess())
                .Returns(new ResponseModel { Success = true });

            _responseFactory.Setup(x => x.CreateSuccess(It.IsAny<long>()))
                .Returns((long id) => new ResponseModel { Success = true, NewEntityId = id });

            _responseFactory.Setup(x => x.CreateFailure(It.IsAny<string>()))
                .Returns((string error) => new ResponseModel { Success = false, Error = error });

            _service = new NotificationService(_notificationRepository.Object, _notificationFactory.Object, _responseFactory.Object);
        }

        [Fact]
        public async Task AddAsync_AddsNotification()
        {
            var notifications = new List<Notification>();

            _notificationFactory.Setup(x => x.Create(It.IsAny<AddNotificationRequest>()))
                .Returns((AddNotificationRequest request) => new Notification
                {
                    UserId = request.UserId,
                    Message = request.Message,
                });

            const long NewId = 1;

            _notificationRepository.Setup(x => x.InsertAsync(It.IsAny<Notification>()))
                .Callback((Notification notification) => notifications.Add(notification))
                .ReturnsAsync(NewId);

            var request = new AddNotificationRequest
            {
                Message = "msg",
                UserId = 2
            };

            var res = await _service.AddAsync(request);

            Assert.True(res.Success);
            Assert.Equal(NewId, res.NewEntityId);
            Assert.Contains(notifications, x => x.UserId == request.UserId && x.Message == request.Message);
        }

        [Fact]
        public async Task AddAsync_ExceptionThrown_Failure()
        {
            const string Error = "err";

            _notificationFactory.Setup(x => x.Create(It.IsAny<AddNotificationRequest>()))
                .Throws(new Exception(Error));

            var res = await _service.AddAsync(new AddNotificationRequest());

            Assert.False(res.Success);
            Assert.Equal(Error, res.Error);
        }

        [Fact]
        public async Task DeleteAsync_DeletesSpecifiedNotification()
        {
            const long IdToDelete = 2;

            var notifications = new List<Notification>
            {
                new Notification { Id = 1, },
                new Notification { Id = IdToDelete },
                new Notification { Id = 3}
            };

            _notificationRepository.Setup(x => x.DeleteByIdAsync(It.IsAny<long>()))
                .Callback((long id) => notifications.RemoveAll(x => x.Id == id));

            var res = await _service.DeleteByIdAsync(IdToDelete);

            Assert.True(res.Success);
            Assert.DoesNotContain(notifications, x => x.Id == IdToDelete);
        }

        [Fact]
        public async Task DeleteAsync_ExceptionThrown_Failure()
        {
            const string Error = "err";

            _notificationRepository.Setup(x => x.DeleteByIdAsync(It.IsAny<long>()))
                .ThrowsAsync(new Exception(Error));

            var res = await _service.DeleteByIdAsync(2137);

            Assert.False(res.Success);  
            Assert.Equal(Error, res.Error);
        }

        [Fact]
        public async Task UpdateAsync_UpdatesNotification()
        {
            var notification = new Notification
            {
                Id = 1,
                IsRead = false
            };

            _notificationRepository.Setup(x => x.UpdateAsync(It.IsAny<UpdateNotificationRequest>()))
                .Callback((UpdateNotificationRequest request) => notification.IsRead = request.IsRead.Value);

            var request = new UpdateNotificationRequest
            {
                Id = 1,
                IsRead = true
            };

            var res = await _service.UpdateAsync(request);

            Assert.True(res.Success);
            Assert.Equal(request.IsRead, notification.IsRead);
        }

        [Fact]
        public async Task UpdateAsync_ExceptionThrown_Failure()
        {
            const string Error = "err";

            _notificationRepository.Setup(x => x.UpdateAsync(It.IsAny<UpdateNotificationRequest>()))
                .ThrowsAsync(new Exception(Error));

            var res = await _service.UpdateAsync(new UpdateNotificationRequest());

            Assert.False(res.Success);
            Assert.Equal(Error, res.Error);
        }
    }
}
