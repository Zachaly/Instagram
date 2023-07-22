using Instagram.Domain.Entity;
using Instagram.Models.DirectMessage;
using Instagram.Models.DirectMessage.Request;
using Instagram.Tests.Integration.ApiTests.Infrastructure;
using System.Net;
using System.Net.Http.Json;

namespace Instagram.Tests.Integration.ApiTests
{
    public class DirectMessageControllerTests : ApiTest
    {
        const string Endpoint = "/api/direct-message";

        [Fact]
        public async Task GetAsync_WithUserIds_ReturnsProperMessages()
        {
            await Authorize();
            const long User1Id = 1;
            const long User2Id = 2;

            var messagesToInsert = FakeDataFactory.GenerateDirectMessages(User1Id, User2Id, 2);
            messagesToInsert.AddRange(FakeDataFactory.GenerateDirectMessages(User2Id, User1Id, 2));
            messagesToInsert.AddRange(FakeDataFactory.GenerateDirectMessages(21, 37, 2));

            Insert("DirectMessage", messagesToInsert);

            var messageIds = GetFromDatabase<long>("SELECT Id FROM DirectMessage WHERE SenderId IN @Ids AND ReceiverId IN @Ids",
                new { Ids = new long[] { User1Id, User2Id } }).ToList();

            var response = await _httpClient.GetAsync($"{Endpoint}?UserIds={User1Id}&UserIds={User2Id}");
            var content = await response.Content.ReadFromJsonAsync<IEnumerable<DirectMessageModel>>();

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.Equivalent(messageIds, content.Select(x => x.Id));
        }

        [Fact]
        public async Task GetByIdAsync_ReturnsMessage()
        {
            await Authorize();

            Insert("DirectMessage", FakeDataFactory.GenerateDirectMessages(1, 2, 3));

            var message = GetFromDatabase<DirectMessage>("SELECT * FROM DirectMessage").First();

            var response = await _httpClient.GetAsync($"{Endpoint}/{message.Id}");
            var content = await response.Content.ReadFromJsonAsync<DirectMessageModel>();

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.Equal(message.Content, content.Content);
            Assert.Equal(message.SenderId, content.SenderId);
            Assert.Equal(message.Id, content.Id);
            Assert.Equal(message.Read, content.Read);
        }

        [Fact]
        public async Task GetByIdAsync_MessageNotFound_Failure()
        {
            await Authorize();

            var response = await _httpClient.GetAsync($"{Endpoint}/2137");

            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        }

        [Fact]
        public async Task GetCountAsync_ReturnsProperCount()
        {
            await Authorize();

            const int Count = 10;

            Insert("DirectMessage", FakeDataFactory.GenerateDirectMessages(1, 2, Count));

            var response = await _httpClient.GetAsync($"{Endpoint}/count");
            var content = await response.Content.ReadFromJsonAsync<int>();

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.Equal(Count, content);
        }

        [Fact]
        public async Task PostAsync_AddsMessageAndNotification()
        {
            await Authorize();

            var usersToInsert = FakeDataFactory.GenerateUsers(2);

            Insert("User", usersToInsert);

            var userIds = GetFromDatabase<long>("SELECT Id FROM [User] WHERE Nickname IN @Names",
                new { Names = usersToInsert.Select(x => x.Nickname) });

            var request = new AddDirectMessageRequest
            {
                SenderId = userIds.First(),
                ReceiverId = userIds.Last(),
                Content = "content"
            };

            var response = await _httpClient.PostAsJsonAsync(Endpoint, request);

            var messages = GetFromDatabase<DirectMessage>("SELECT * FROM DirectMessage");

            Assert.Equal(HttpStatusCode.Created, response.StatusCode);
            Assert.Contains(messages, msg => msg.Content == request.Content && msg.SenderId == request.SenderId && msg.ReceiverId == request.ReceiverId);
            Assert.Contains(GetFromDatabase<Notification>("SELECT * FROM Notification"), x => x.UserId == request.ReceiverId);
        }

        [Fact]
        public async Task PostAsync_InvalidRequest_ReturnsErrors()
        {
            await Authorize();

            var request = new AddDirectMessageRequest
            {
                SenderId = 0,
                ReceiverId = 2,
                Content = "content"
            };

            var response = await _httpClient.PostAsJsonAsync(Endpoint, request);

            var content = await ReadErrorResponse(response);

            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
            Assert.Contains(content.ValidationErrors.Keys, x => x == "SenderId");
        }

        [Fact]
        public async Task PatchAsync_UpdatesMessage()
        {
            await Authorize();

            Insert("DirectMessage", FakeDataFactory.GenerateDirectMessages(1, 2, 1));

            var messageId = GetFromDatabase<long>("SELECT Id FROM DirectMessage").First();

            var request = new UpdateDirectMessageRequest
            {
                Id = messageId,
                Read = true
            };

            var response = await _httpClient.PatchAsJsonAsync(Endpoint, request);

            var updatedMessage = GetFromDatabase<DirectMessage>("SELECT * FROM DirectMessage").First();

            Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);
            Assert.Equal(request.Read, updatedMessage.Read);
        }

        [Fact]
        public async Task PatchAsync_InvalidRequest_ReturnsErrors()
        {
            await Authorize();

            Insert("DirectMessage", FakeDataFactory.GenerateDirectMessages(1, 2, 1));

            var messageId = GetFromDatabase<long>("SELECT Id FROM DirectMessage").First();

            var request = new UpdateDirectMessageRequest
            {
                Id = messageId,
                Content = ""
            };

            var response = await _httpClient.PatchAsJsonAsync(Endpoint, request);

            var content = await ReadErrorResponse(response);
            
            var updatedMessage = GetFromDatabase<DirectMessage>("SELECT * FROM DirectMessage").First();

            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
            Assert.Contains(content.ValidationErrors.Keys, x => x == "Content");
            Assert.NotEqual(request.Content, updatedMessage.Content);
        }

        [Fact]
        public async Task DeleteAsync_DeletesMessage()
        {
            await Authorize();

            const int InitialCount = 10;

            Insert("DirectMessage", FakeDataFactory.GenerateDirectMessages(1, 2, InitialCount));

            var messageId = GetFromDatabase<long>("SELECT Id FROM DirectMessage").Last();

            var response = await _httpClient.DeleteAsync($"{Endpoint}/{messageId}");

            var messageIds = GetFromDatabase<long>("SELECT Id FROM DirectMessage");

            Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);
            Assert.DoesNotContain(messageIds, x => x == messageId);
            Assert.Equal(InitialCount - 1, messageIds.Count());
        }
    }
}
