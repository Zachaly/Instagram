using Instagram.Database.Repository;
using Instagram.Database.Sql;
using Instagram.Domain.Entity;
using Instagram.Models.Notification.Request;

namespace Instagram.Tests.Integration.DatabaseTests
{
    public class NotificationRepositoryTests : RepositoryTest
    {
        private readonly NotificationRepository _repository;

        public NotificationRepositoryTests() : base()
        {
            _repository = new NotificationRepository(new SqlQueryBuilder(), _connectionFactory);
        }

        [Fact]
        public async Task UpdateAsync_UpdatesProperEntity()
        {
            Insert("Notification", FakeDataFactory.GenerateNotifications(1, 5));

            var id = GetFromDatabase<long>("SELECT Id FROM [Notification]").Last();

            var request = new UpdateNotificationRequest
            {
                Id = id,
                IsRead = true,
            };

            await _repository.UpdateAsync(request);

            var notifications = GetFromDatabase<Notification>("SELECT * FROM Notification");

            Assert.All(notifications.Where(x => x.Id == id), notification =>
            {
                Assert.Equal(request.IsRead, notification.IsRead);
            });
            Assert.All(notifications.Where(x => x.Id != id), notification =>
            {
                Assert.NotEqual(request.IsRead, notification.IsRead);
            });
        }
    }
}
