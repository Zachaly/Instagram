using Instagram.Domain.Entity;
using Instagram.Models.PostComment;
using Instagram.Models.PostComment.Request;
using Instagram.Tests.Integration.ApiTests.Infrastructure;
using Microsoft.AspNetCore.Hosting;
using System.Net;
using System.Net.Http.Json;

namespace Instagram.Tests.Integration.ApiTests
{
    public class PostCommentControllerTest : ApiTest
    {
        const string Endpoint = "/api/post-comment";

        [Fact]
        public async Task GetAsync_ReturnsComments()
        {
            Insert("User", FakeDataFactory.GenerateUsers(1));

            var user = GetFromDatabase<User>("SELECT * FROM [User]").First();

            Insert("PostComment", FakeDataFactory.GeneratePostComments(user.Id, 5));

            var comments = GetFromDatabase<PostComment>("SELECT * FROM PostComment");

            var response = await _httpClient.GetAsync(Endpoint);
            var content = await response.Content.ReadFromJsonAsync<IEnumerable<PostCommentModel>>();

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.Equivalent(comments.Select(x => x.Id), content.Select(x => x.Id));
            Assert.All(content, comment => Assert.Equal(user.Nickname, comment.UserName));
        }

        [Fact]
        public async Task GetByIdAsync_ReturnsComment()
        {
            Insert("User", FakeDataFactory.GenerateUsers(1));

            var user = GetFromDatabase<User>("SELECT * FROM [User]").First();

            Insert("PostComment", FakeDataFactory.GeneratePostComments(user.Id, 1));

            var comment = GetFromDatabase<PostComment>("SELECT * FROM PostComment").First();

            var response = await _httpClient.GetAsync($"{Endpoint}/{comment.Id}");
            var content = await response.Content.ReadFromJsonAsync<PostCommentModel>();

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.Equal(comment.Content, content.Content);
            Assert.Equal(comment.Created, content.Created);
            Assert.Equal(user.Nickname, content.UserName);
            Assert.Equal(user.Id, content.UserId);
        }

        [Fact]
        public async Task GetByIdAsync_CommentNotFound_Failure()
        {
            var response = await _httpClient.GetAsync($"{Endpoint}/2137");

            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        }

        [Fact]
        public async Task GetCountAsync_ReturnsCount()
        {
            const int Count = 15;

            Insert("PostComment", FakeDataFactory.GeneratePostComments(1, Count));

            var response = await _httpClient.GetAsync($"{Endpoint}/count");
            var content = await response.Content.ReadFromJsonAsync<int>();

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.Equal(Count, content);
        }

        [Fact]
        public async Task PostAsync_AddsComment()
        {
            await Authorize();

            var request = new AddPostCommentRequest
            {
                Content = "content",
                PostId = 1,
                UserId = 2
            };

            var response = await _httpClient.PostAsJsonAsync(Endpoint, request);

            var comments = GetFromDatabase<PostComment>("SELECT * FROM PostComment");

            Assert.Equal(HttpStatusCode.Created, response.StatusCode);
            Assert.Contains(comments, x => x.PostId == request.PostId && x.UserId == request.UserId && x.Content == request.Content);
            Assert.Single(comments);
        }

        [Fact]
        public async Task PostAsync_InvalidRequest_ReturnsErrors()
        {
            await Authorize();

            var request = new AddPostCommentRequest
            {
                Content = "",
                PostId = 1,
                UserId = 2
            };

            var response = await _httpClient.PostAsJsonAsync(Endpoint, request);
            var content = await ReadErrorResponse(response);

            var comments = GetFromDatabase<PostComment>("SELECT * FROM PostComment");

            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
            Assert.Empty(comments);
            Assert.Contains(content.ValidationErrors.Keys, x => x == "Content");
        }
    }
}
