using Instagram.Domain.Entity;
using Instagram.Models.Notification;
using Instagram.Models.Notification.Request;
using Instagram.Tests.Integration.ApiTests.Infrastructure;
using System.Net;
using System.Net.Http.Json;

namespace Instagram.Tests.Integration.ApiTests
{
    public class NotificationControllerTests : ApiTest
    {
        const string Endpoint = "/api/notification";

        [Fact]
        public async Task GetAsync_ReturnsNotifications()
        {
            await Authorize();

            Insert("Notification", FakeDataFactory.GenerateNotifications(1, 5));

            var notificationIds = GetFromDatabase<long>("SELECT Id FROM Notification");

            var response = await _httpClient.GetAsync(Endpoint);
            var content = await response.Content.ReadFromJsonAsync<IEnumerable<NotificationModel>>();

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.Equivalent(notificationIds, content.Select(x => x.Id));
        }

        [Fact]
        public async Task GetByIdAsync_ReturnsSpecifiedNotification()
        {
            await Authorize();

            Insert("Notification", FakeDataFactory.GenerateNotifications(1, 3));

            var notification = GetFromDatabase<Notification>("SELECT * FROM Notification").Last();

            var response = await _httpClient.GetAsync($"{Endpoint}/{notification.Id}");
            var content = await response.Content.ReadFromJsonAsync<NotificationModel>();

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.Equal(notification.Id, content.Id);
            Assert.Equal(notification.Message, content.Message);
            Assert.Equal(notification.IsRead, content.IsRead);
            Assert.Equal(notification.Created, content.Created);
        }

        [Fact]
        public async Task GetByIdAsync_NotificationNotFound_Failure()
        {
            await Authorize();

            var response = await _httpClient.GetAsync($"{Endpoint}/2137");

            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        }

        [Fact]
        public async Task GetCountAsync_ReturnsProperCount()
        {
            await Authorize();

            const int Count = 20;

            Insert("Notification", FakeDataFactory.GenerateNotifications(1, Count));

            var response = await _httpClient.GetAsync($"{Endpoint}/count");
            var content = await response.Content.ReadFromJsonAsync<int>();

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.Equal(Count, content);
        }

        [Fact]
        public async Task PostAsync_AddsNotification()
        {
            await Authorize();

            var request = new AddNotificationRequest
            {
                Message = "message",
                UserId = 1
            };

            var response = await _httpClient.PostAsJsonAsync(Endpoint, request);

            var notifications = GetFromDatabase<Notification>("SELECT * FROM Notification");

            Assert.Equal(HttpStatusCode.Created, response.StatusCode);
            Assert.Contains(notifications, x => x.UserId == request.UserId && x.Message == request.Message);
        }

        [Fact]
        public async Task PostAsync_InvalidRequest_Failure()
        {
            await Authorize();

            var request = new AddNotificationRequest
            {
                UserId = 1,
                Message = "",
            };

            var response = await _httpClient.PostAsJsonAsync(Endpoint, request);
            var content = await ReadErrorResponse(response);

            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
            Assert.Contains(content.ValidationErrors.Keys, x => x == "Message");
        }

        [Fact]
        public async Task DeleteByIdAsync_DeletesSpecifiedNotification()
        {
            await Authorize();

            Insert("Notification", FakeDataFactory.GenerateNotifications(1, 5));

            var idToDelete = GetFromDatabase<long>("SELECT Id FROM Notification").Last();

            var response = await _httpClient.DeleteAsync($"{Endpoint}/{idToDelete}");

            var notifications = GetFromDatabase<Notification>("SELECT * FROM Notification");

            Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);
            Assert.DoesNotContain(notifications, x => x.Id == idToDelete);
        }

        [Fact]
        public async Task UpdateAsync_UpdatesSpecifiedNotification()
        {
            await Authorize();

            Insert("Notification", FakeDataFactory.GenerateNotifications(1, 5));

            var notificationId = GetFromDatabase<long>("SELECT Id FROM Notification").Last();

            var request = new UpdateNotificationRequest
            {
                Id = notificationId,
                IsRead = true,
            };

            var response = await _httpClient.PatchAsJsonAsync(Endpoint, request);

            var notifications = GetFromDatabase<Notification>("SELECT * FROM Notification");

            Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);
            Assert.All(notifications.Where(x => x.Id == request.Id), notification =>
            {
                Assert.Equal(request.IsRead, notification.IsRead);
            });
            Assert.All(notifications.Where(x => x.Id != request.Id), notification =>
            {
                Assert.NotEqual(request.IsRead, notification.IsRead);
            });
        }

        [Fact]
        public async Task UpdateAsync_InvalidRequest_Failure()
        {
            await Authorize();

            var request = new UpdateNotificationRequest
            {
                Id = 0
            };

            var response = await _httpClient.PatchAsJsonAsync(Endpoint, request);
            var content = await ReadErrorResponse(response);

            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
            Assert.Contains(content.ValidationErrors.Keys, x => x == "Id");
        }
    }
}
