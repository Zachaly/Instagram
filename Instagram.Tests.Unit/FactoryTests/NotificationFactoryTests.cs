using Instagram.Application;
using Instagram.Models.Notification.Request;

namespace Instagram.Tests.Unit.FactoryTests
{
    public class NotificationFactoryTests
    {
        private readonly NotificationFactory _factory;

        public NotificationFactoryTests()
        {
            _factory = new NotificationFactory();
        }

        [Fact]
        public void Create_CreatesValidEntity()
        {
            var request = new AddNotificationRequest
            {
                Message = "msg",
                UserId = 1,
            };

            var notification = _factory.Create(request);

            Assert.Equal(request.Message, notification.Message);
            Assert.Equal(request.UserId, notification.UserId);
            Assert.NotEqual(default, notification.Created);
            Assert.False(notification.IsRead);
        }
    }
}
