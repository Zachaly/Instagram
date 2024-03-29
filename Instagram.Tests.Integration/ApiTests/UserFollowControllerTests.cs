﻿using Instagram.Domain.Entity;
using Instagram.Models.UserFollow;
using Instagram.Models.UserFollow.Request;
using Instagram.Tests.Integration.ApiTests.Infrastructure;
using System.Net;
using System.Net.Http.Json;

namespace Instagram.Tests.Integration.ApiTests
{
    public class UserFollowControllerTests : ApiTest
    {
        const string Endpoint = "/api/user-follow";

        [Fact]
        public async Task GetAsync_NoConditionalJoins_ReturnsFollows()
        {
            Insert("User", FakeDataFactory.GenerateUsers(2));

            var users = GetFromDatabase<User>("SELECT * FROM [User]");

            var follows = new UserFollow[]
            {
                new UserFollow { FollowedUserId = users.First().Id, FollowingUserId = users.Last().Id },
                new UserFollow { FollowedUserId = users.Last().Id, FollowingUserId = users.First().Id },
            };

            Insert("UserFollow", follows);

            var response = await _httpClient.GetAsync(Endpoint);
            var content = await response.Content.ReadFromJsonAsync<IEnumerable<UserFollowModel>>();

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.Equivalent(content.Select(x => x.FollowedUserId), follows.Select(x => x.FollowedUserId));
            Assert.Equivalent(content.Select(x => x.FollowingUserId), follows.Select(x => x.FollowingUserId));
            Assert.All(content, follow => Assert.Null(follow.UserName));
        }

        [Fact]
        public async Task GetAsync_WithConditionalJoins_ReturnsFollowsWithJoinedUserName()
        {
            Insert("User", FakeDataFactory.GenerateUsers(2));

            var users = GetFromDatabase<User>("SELECT * FROM [User]");

            var follows = new UserFollow[]
            {
                new UserFollow { FollowedUserId = users.First().Id, FollowingUserId = users.Last().Id },
                new UserFollow { FollowedUserId = users.Last().Id, FollowingUserId = users.First().Id },
            };

            Insert("UserFollow", follows);

            var response = await _httpClient.GetAsync($"{Endpoint}?JoinFollower=true");
            var content = await response.Content.ReadFromJsonAsync<IEnumerable<UserFollowModel>>();

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.Equivalent(content.Select(x => x.FollowedUserId), follows.Select(x => x.FollowedUserId));
            Assert.Equivalent(content.Select(x => x.FollowingUserId), follows.Select(x => x.FollowingUserId));
            Assert.Contains(content, x => x.FollowingUserId == users.First().Id && x.UserName == users.First().Nickname);
            Assert.Contains(content, x => x.FollowingUserId == users.Last().Id && x.UserName == users.Last().Nickname);
        }

        [Fact]
        public async Task GetCountAsync_ReturnsProperCount()
        {
            const int Count = 20;

            Insert("UserFollow", FakeDataFactory.GenerateFollows(Count));

            var response = await _httpClient.GetAsync($"{Endpoint}/count");
            var content = await response.Content.ReadFromJsonAsync<int>();

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.Equal(Count, content);
        }

        [Fact]
        public async Task PostAsync_AddsFollowAndNotification()
        {
            await Authorize();

            var usersToInsert = FakeDataFactory.GenerateUsers(2);

            Insert("User", usersToInsert);

            var userIds = GetFromDatabase<long>("SELECT Id FROM [User] WHERE Nickname IN @Names",
                new { Names = usersToInsert.Select(x => x.Nickname) });

            var request = new AddUserFollowRequest
            {
                FollowedUserId = userIds.First(),
                FollowingUserId = userIds.Last()
            };

            var response = await _httpClient.PostAsJsonAsync(Endpoint, request);

            var follows = GetFromDatabase<UserFollow>("SELECT * FROM [UserFollow]");
            
            Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);
            Assert.Contains(follows, x => x.FollowedUserId == request.FollowedUserId && x.FollowingUserId == request.FollowingUserId);
            Assert.Single(follows);
            Assert.Contains(GetFromDatabase<Notification>("SELECT * FROM Notification"), x => x.UserId == request.FollowedUserId);
        }

        [Fact]
        public async Task PostAsync_InvalidRequest_ReturnsErrors()
        {
            await Authorize();

            var request = new AddUserFollowRequest
            {
                FollowedUserId = 0,
                FollowingUserId = 3
            };

            var response = await _httpClient.PostAsJsonAsync(Endpoint, request);
            var content = await ReadErrorResponse(response);

            var follows = GetFromDatabase<UserFollow>("SELECT * FROM [UserFollow]");

            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
            Assert.Contains(content.ValidationErrors.Keys, x => x == "FollowedUserId");
            Assert.DoesNotContain(follows, x => x.FollowedUserId == request.FollowedUserId && x.FollowingUserId == request.FollowingUserId);
            Assert.Empty(follows);
        }

        [Fact]
        public async Task DeleteAsync_DeletesProperFollow()
        {
            await Authorize();

            Insert("UserFollow", FakeDataFactory.GenerateFollows(20));

            var followToDelete = GetFromDatabase<UserFollow>("SELECT * FROM [UserFollow]").First();

            var response = await _httpClient.DeleteAsync($"{Endpoint}/{followToDelete.FollowingUserId}/{followToDelete.FollowedUserId}");

            var follows = GetFromDatabase<UserFollow>("SELECT * FROM UserFollow");

            Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);
            Assert.DoesNotContain(follows, x => x.FollowedUserId == followToDelete.FollowedUserId 
                && x.FollowingUserId == followToDelete.FollowingUserId);
        }
    }
}
