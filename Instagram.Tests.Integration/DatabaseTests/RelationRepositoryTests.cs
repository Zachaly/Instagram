using Instagram.Database.Repository;
using Instagram.Database.Sql;
using Instagram.Domain.Entity;
using Instagram.Models.Relation.Request;
using System.Collections.Immutable;

namespace Instagram.Tests.Integration.DatabaseTests
{
    public class RelationRepositoryTests : RepositoryTest
    {
        private readonly RelationRepository _repository;

        public RelationRepositoryTests() : base()
        {
            _repository = new RelationRepository(new SqlQueryBuilder(), _connectionFactory);
        }

        [Fact]
        public async Task GetAsync_ImageIdsMappedProperly()
        {
            Insert("User", FakeDataFactory.GenerateUsers(2));

            var user1 = GetFromDatabase<User>("SELECT * FROM [User]").First();
            var user2 = GetFromDatabase<User>("SELECT * FROM [User]").Last();

            var relationsToInsert = FakeDataFactory.GenerateRelations(user1.Id, 2);
            relationsToInsert.AddRange(FakeDataFactory.GenerateRelations(user2.Id, 2));

            Insert("Relation", relationsToInsert);

            var relations = GetFromDatabase<Relation>("SELECT * FROM [Relation]");

            const int ImageCount = 4;

            var imagesToInsert = relations.SelectMany(relation => FakeDataFactory.GenerateRelationImages(relation.Id, ImageCount));

            Insert("RelationImage", imagesToInsert);

            var images = GetFromDatabase<RelationImage>("SELECT * FROM [RelationImage]");

            var res = await _repository.GetAsync(new GetRelationRequest());

            Assert.All(res.Where(r => r.UserId == user1.Id), relation =>
            {
                Assert.Equal(user1.Nickname, relation.UserName);
            });
            Assert.All(res.Where(r => r.UserId == user2.Id), relation =>
            {
                Assert.Equal(user2.Nickname, relation.UserName);
            });
            Assert.Equivalent(relations.Select(x => x.Id), res.Select(x => x.Id));
            Assert.All(res, relation =>
            {
                Assert.Equal(ImageCount, relation.ImageIds.Count());
            });
            Assert.All(res, relation =>
            {
                Assert.Equivalent(relation.ImageIds, images.Where(x => x.RelationId == relation.Id).Select(x => x.Id));
            });
        }

        [Fact]
        public async Task GetByIdAsync_ImageIdsMappedProperly()
        {
            Insert("User", FakeDataFactory.GenerateUsers(1));

            var user = GetFromDatabase<User>("SELECT * FROM [User]").First();

            Insert("Relation", FakeDataFactory.GenerateRelations(user.Id, 1));

            var relation = GetFromDatabase<Relation>("SELECT * FROM [Relation]").First();

            Insert("RelationImage", FakeDataFactory.GenerateRelationImages(relation.Id, 5));

            var images = GetFromDatabase<RelationImage>("SELECT * FROM [RelationImage]");

            var res = await _repository.GetByIdAsync(relation.Id);

            Assert.Equivalent(res.ImageIds, images.Select(x => x.Id));
            Assert.Equal(user.Nickname, res.UserName);
        }
    }
}
