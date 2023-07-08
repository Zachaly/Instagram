using Instagram.Domain.Entity;
using Instagram.Models.PostTag;
using Instagram.Models.PostTag.Request;
using Instagram.Tests.Integration.ApiTests.Infrastructure;
using System.Net;
using System.Net.Http.Json;

namespace Instagram.Tests.Integration.ApiTests
{
    public class PostTagControllerTests : ApiTest
    {
        const string Endpoint = "/api/post-tag";

        [Fact]
        public async Task GetAsync_ReturnsTags()
        {
            var tags = FakeDataFactory.GeneratePostTags(1, 5);

            Insert("PostTag", tags);

            var response = await _httpClient.GetAsync(Endpoint);
            var content = await response.Content.ReadFromJsonAsync<IEnumerable<PostTagModel>>();

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.Equivalent(tags.Select(x => x.PostId), content.Select(x => x.PostId));
            Assert.Equivalent(tags.Select(x => x.Tag), content.Select(x => x.Tag));
        }

        [Fact]
        public async Task GetCountAsync_ReturnsProperCount()
        {
            const int Count = 20;

            Insert("PostTag", FakeDataFactory.GeneratePostTags(1, Count));

            var response = await _httpClient.GetAsync($"{Endpoint}/count");
            var content = await response.Content.ReadFromJsonAsync<int>();

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.Equal(Count, content);
        }

        [Fact]
        public async Task AddAsync_AddsTag()
        {
            await Authorize();

            var request = new AddPostTagRequest
            {
                PostId = 1,
                Tags = new string[] { "tag1", "tag2" }
            };

            var response = await _httpClient.PostAsJsonAsync(Endpoint, request);

            var tags = GetFromDatabase<PostTag>("SELECT * FROM PostTag");

            Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);
            Assert.Equivalent(request.Tags, tags.Select(x => x.Tag));
        }

        [Fact]
        public async Task AddAsync_InvalidRequest_ReturnsErrors()
        {
            await Authorize();

            var request = new AddPostTagRequest
            {
                PostId = 1,
                Tags = new string[] { "tag1", "tag2", new string('a', 51) }
            };

            var response = await _httpClient.PostAsJsonAsync(Endpoint, request);
            var content = await ReadErrorResponse(response);

            var tags = GetFromDatabase<PostTag>("SELECT * FROM PostTag");

            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
            Assert.Contains(content.ValidationErrors.Keys, x => x == "Tags");
            Assert.Empty(tags);
        }

        [Fact]
        public async Task DeleteAsync_DeletesProperTag()
        {
            await Authorize();

            const long PostIdToDelete = 1;
            const string TagToDelete = "aaaaa";

            var tags = FakeDataFactory.GeneratePostTags(PostIdToDelete, 2);
            tags.AddRange(FakeDataFactory.GeneratePostTags(2, 2));
            tags.Add(new PostTag { PostId = PostIdToDelete, Tag = TagToDelete });

            Insert("PostTag", tags);

            var response = await _httpClient.DeleteAsync($"{Endpoint}/{PostIdToDelete}/{TagToDelete}");

            var currentTags = GetFromDatabase<PostTag>("SELECT * FROM PostTag");

            Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);
            Assert.DoesNotContain(currentTags, x => x.PostId == PostIdToDelete && x.Tag == TagToDelete);
        }
    }
}
