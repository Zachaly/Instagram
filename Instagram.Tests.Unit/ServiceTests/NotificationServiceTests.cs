using Instagram.Application;
using Instagram.Application.Abstraction;
using Instagram.Database.Repository;
using Instagram.Domain.Entity;
using Instagram.Models.Notification.Request;
using Instagram.Models.Response;
using NSubstitute;
using NSubstitute.ExceptionExtensions;

namespace Instagram.Tests.Unit.ServiceTests
{
    public class NotificationServiceTests
    {
        private readonly INotificationRepository _notificationRepository;
        private readonly INotificationFactory _notificationFactory;
        private readonly IResponseFactory _responseFactory;
        private readonly NotificationService _service;

        public NotificationServiceTests()
        {
            _notificationRepository = Substitute.For<INotificationRepository>();
            _notificationFactory = Substitute.For<INotificationFactory>();
            _responseFactory = ResponseFactoryMock.Create();

            _service = new NotificationService(_notificationRepository, _notificationFactory, _responseFactory);
        }

        [Fact]
        public async Task AddAsync_AddsNotification()
        {
            var notifications = new List<Notification>();

            _notificationFactory.Create(Arg.Any<AddNotificationRequest>())
                .Returns(info => new Notification
                {
                    UserId = info.Arg<AddNotificationRequest>().UserId,
                    Message = info.Arg<AddNotificationRequest>().Message,
                });

            const long NewId = 1;

            _notificationRepository.InsertAsync(Arg.Any<Notification>())
                .Returns(NewId)
                .AndDoes(info => notifications.Add(info.Arg<Notification>()));

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

            _notificationFactory.Create(Arg.Any<AddNotificationRequest>())
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

            _notificationRepository.DeleteByIdAsync(Arg.Any<long>())
                .Returns(Task.CompletedTask)
                .AndDoes(info => notifications.RemoveAll(n => n.Id == info.Arg<long>()));

            var res = await _service.DeleteByIdAsync(IdToDelete);

            Assert.True(res.Success);
            Assert.DoesNotContain(notifications, x => x.Id == IdToDelete);
        }

        [Fact]
        public async Task DeleteAsync_ExceptionThrown_Failure()
        {
            const string Error = "err";
            
            _notificationRepository.DeleteByIdAsync(Arg.Any<long>())
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

            _notificationRepository.UpdateAsync(Arg.Any<UpdateNotificationRequest>())
                .Returns(Task.CompletedTask)
                .AndDoes(info => notification.IsRead = info.Arg<UpdateNotificationRequest>().IsRead.Value);

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

            _notificationRepository.UpdateAsync(Arg.Any<UpdateNotificationRequest>())
                .ThrowsAsync(new Exception(Error));

            var res = await _service.UpdateAsync(new UpdateNotificationRequest());

            Assert.False(res.Success);
            Assert.Equal(Error, res.Error);
        }
    }
}
