using Instagram.Database.Sql;
using Instagram.Domain.Entity;
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
            foreach (var user in FakeDataFactory.GenerateUsers(2))
            {
                var query = new SqlQueryBuilder().BuildInsert("User", user).Build();
                ExecuteQuery(query, user);
            }

            var users = GetFromDatabase<User>("SELECT * FROM [User]");

            var follows = new UserFollow[]
            {
                new UserFollow { FollowedUserId = users.First().Id, FollowingUserId = users.Last().Id },
                new UserFollow { FollowedUserId = users.Last().Id, FollowingUserId = users.First().Id },
            };

            foreach(var follow in follows)
            {
                var query = new SqlQueryBuilder().BuildInsert("UserFollow", follow).Build();
                ExecuteQuery(query, follow);
            }

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
            foreach (var user in FakeDataFactory.GenerateUsers(2))
            {
                var query = new SqlQueryBuilder().BuildInsert("User", user).Build();
                ExecuteQuery(query, user);
            }

            var users = GetFromDatabase<User>("SELECT * FROM [User]");

            var follows = new UserFollow[]
            {
                new UserFollow { FollowedUserId = users.First().Id, FollowingUserId = users.Last().Id },
                new UserFollow { FollowedUserId = users.Last().Id, FollowingUserId = users.First().Id },
            };

            foreach (var follow in follows)
            {
                var query = new SqlQueryBuilder().BuildInsert("UserFollow", follow).Build();
                ExecuteQuery(query, follow);
            }

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

            foreach(var follow in FakeDataFactory.GenerateFollows(Count))
            {
                var query = new SqlQueryBuilder().BuildInsert("UserFollow", follow).Build();
                ExecuteQuery(query, follow);
            }

            var response = await _httpClient.GetAsync($"{Endpoint}/count");
            var content = await response.Content.ReadFromJsonAsync<int>();

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.Equal(Count, content);
        }

        [Fact]
        public async Task PostAsync_AddsFollow()
        {
            await Authorize();

            var request = new AddUserFollowRequest
            {
                FollowedUserId = 2,
                FollowingUserId = 3
            };

            var response = await _httpClient.PostAsJsonAsync(Endpoint, request);

            var follows = GetFromDatabase<UserFollow>("SELECT * FROM [UserFollow]");
            
            Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);
            Assert.Contains(follows, x => x.FollowedUserId == request.FollowedUserId && x.FollowingUserId == request.FollowingUserId);
            Assert.Single(follows);
        }

        [Fact]
        public async Task DeleteAsync_DeletesProperFollow()
        {
            await Authorize();

            foreach (var follow in FakeDataFactory.GenerateFollows(20))
            {
                var query = new SqlQueryBuilder().BuildInsert("UserFollow", follow).Build();
                ExecuteQuery(query, follow);
            }

            var followToDelete = GetFromDatabase<UserFollow>("SELECT * FROM [UserFollow]").First();

            var response = await _httpClient.DeleteAsync($"{Endpoint}/{followToDelete.FollowingUserId}/{followToDelete.FollowedUserId}");

            var follows = GetFromDatabase<UserFollow>("SELECT * FROM UserFollow");

            Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);
            Assert.DoesNotContain(follows, x => x.FollowedUserId == followToDelete.FollowedUserId 
                && x.FollowingUserId == followToDelete.FollowingUserId);
        }
    }
}
