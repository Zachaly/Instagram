﻿using Instagram.Domain.Entity;
using Instagram.Models.PostLike;
using Instagram.Models.PostLike.Request;
using Instagram.Tests.Integration.ApiTests.Infrastructure;
using System.Net;
using System.Net.Http.Json;

namespace Instagram.Tests.Integration.ApiTests
{
    public class PostLikeControllerTests : ApiTest
    {
        const string Endpoint = "/api/post-like";

        [Fact]
        public async Task GetAsync_ReturnsLikes()
        {
            foreach(var user in FakeDataFactory.GenerateUsers(2))
            {
                Insert("User", user);
            }

            var users = GetFromDatabase<User>("SELECT * FROM [User]");

            var likes = FakeDataFactory.GeneratePostLikes(new long[] { 1, 2, 3, 4 }, users.Select(x => x.Id));

            foreach(var like in likes) 
            {
                Insert("PostLike", like);
            }

            var response = await _httpClient.GetAsync(Endpoint);
            var content = await response.Content.ReadFromJsonAsync<IEnumerable<PostLikeModel>>();

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.All(content, model => Assert.Equal(users.First(x => x.Id == model.UserId).Nickname, model.UserName));
        }

        [Fact]
        public async Task GetCountAsync_ReturnsProperCount()
        {
            var likes = FakeDataFactory.GeneratePostLikes(new long[] { 1, 2, 3, 4 }, new long[] { 1, 2, 3, 4 });

            foreach (var like in likes)
            {
                Insert("PostLike", like);
            }

            var response = await _httpClient.GetAsync($"{Endpoint}/count");
            var content = await response.Content.ReadFromJsonAsync<int>();

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.Equal(likes.Count, content);
        }

        [Fact]
        public async Task PostAsync_AddsPostLike()
        {
            await Authorize();

            var request = new AddPostLikeRequest { PostId = 1, UserId = 2 };

            var response = await _httpClient.PostAsJsonAsync(Endpoint, request);

            var likes = GetFromDatabase<PostLike>("SELECT * FROM PostLike");
            
            Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);
            Assert.Single(likes);
            Assert.Contains(likes, x => x.PostId == request.PostId && x.UserId == request.UserId);
        }

        [Fact]
        public async Task DeleteAsync_DeletesSpecifiedPostLike()
        {
            await Authorize();

            const long PostId = 2;
            const long UserId = 3;

            foreach (var like in FakeDataFactory.GeneratePostLikes(new long[] { 1, PostId, 3, 4 }, new long[] { 1, 2, UserId, 4 }))
            {
                Insert("PostLike", like);
            }

            var response = await _httpClient.DeleteAsync($"{Endpoint}/{PostId}/{UserId}");

            var likes = GetFromDatabase<PostLike>("SELECT * FROM PostLike");

            Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);
            Assert.DoesNotContain(likes, x => x.PostId == PostId && x.UserId == UserId);
        }
    }
}
