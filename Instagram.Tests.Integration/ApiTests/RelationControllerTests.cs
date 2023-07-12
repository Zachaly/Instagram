using Instagram.Domain.Entity;
using Instagram.Models.Relation;
using Instagram.Tests.Integration.ApiTests.Infrastructure;
using System.Net;
using System.Net.Http.Json;

namespace Instagram.Tests.Integration.ApiTests
{
    public class RelationControllerTests : ApiTest
    {
        const string Endpoint = "/api/relation";

        [Fact]
        public async Task GetAsync_ReturnsRelations()
        {
            Insert("User", FakeDataFactory.GenerateUsers(1));

            var userId = GetFromDatabase<long>("SELECT Id FROM [User]").Last();

            Insert("Relation", FakeDataFactory.GenerateRelations(userId, 4));

            var relationIds = GetFromDatabase<long>("SELECT Id FROM [Relation]");

            Insert("RelationImage", relationIds.SelectMany(id => FakeDataFactory.GenerateRelationImages(id, 4)));

            var images = GetFromDatabase<RelationImage>("SELECT * FROM [RelationImage]");

            var response = await _httpClient.GetAsync(Endpoint);
            var content = await response.Content.ReadFromJsonAsync<IEnumerable<RelationModel>>();

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.Equivalent(relationIds, content.Select(x => x.Id));
            Assert.All(content, model =>
            {
                Assert.Equivalent(model.ImageIds, images.Where(x => x.RelationId == model.Id).Select(x => x.Id));
            });
        }

        [Fact]
        public async Task GetByIdAsync_ReturnsSpecifiedRelation()
        {
            Insert("User", FakeDataFactory.GenerateUsers(1));

            var user = GetFromDatabase<User>("SELECT * FROM [User]").Last();

            Insert("Relation", FakeDataFactory.GenerateRelations(user.Id, 4));

            var relations = GetFromDatabase<Relation>("SELECT * FROM [Relation]");

            Insert("RelationImage", relations.SelectMany(x => FakeDataFactory.GenerateRelationImages(x.Id, 4)));

            var images = GetFromDatabase<RelationImage>("SELECT * FROM [RelationImage]");

            var relationToGet = relations.Last();

            var response = await _httpClient.GetAsync($"{Endpoint}/{relationToGet.Id}");
            var content = await response.Content.ReadFromJsonAsync<RelationModel>();

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.Equivalent(images.Where(x => x.RelationId == relationToGet.Id).Select(x => x.Id), content.ImageIds);
            Assert.Equal(relationToGet.Id, content.Id);
            Assert.Equal(relationToGet.Name, content.Name);
            Assert.Equal(user.Nickname, content.UserName);
        }

        [Fact]
        public async Task GetByIdAsync_RelationNotFound_BadRequest()
        {
            var response = await _httpClient.GetAsync($"{Endpoint}/2137");

            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        }

        [Fact]
        public async Task GetCountAsync_ReturnsProperCount()
        {
            const int Count = 20;

            Insert("Relation", FakeDataFactory.GenerateRelations(1, Count));

            var response = await _httpClient.GetAsync($"{Endpoint}/count");
            var content = await response.Content.ReadFromJsonAsync<int>();

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.Equal(Count, content);
        }
    }
}
