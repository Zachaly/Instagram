using Instagram.Database.Repository;
using Instagram.Database.Sql;
using Instagram.Domain.Entity;

namespace Instagram.Tests.Integration.DatabaseTests
{
    public class PostTagRepositoryTests : RepositoryTest
    {
        private readonly PostTagRepository _repository;

        public PostTagRepositoryTests() : base() 
        {
            _repository = new PostTagRepository(new SqlQueryBuilder(), _connectionFactory);
        }

        [Fact]
        public async Task DeleteAsync_DeletesProperEntity()
        {
            var tags = FakeDataFactory.GeneratePostTags(1, 5);
            tags.AddRange(FakeDataFactory.GeneratePostTags(2, 5));

            Insert("PostTag", tags);

            var tagToDelete = tags[new Random().Next(0, tags.Count)];

            await _repository.DeleteAsync(tagToDelete.PostId, tagToDelete.Tag);

            var currentTags = GetFromDatabase<PostTag>("SELECT * FROM PostTag");

            Assert.DoesNotContain(currentTags, x => x.PostId == tagToDelete.PostId && x.Tag == tagToDelete.Tag);
        }

        [Fact]
        public async Task DeleteByPostIdAsync_DeletesProperEntities()
        {
            const long PostId = 2;

            var tags = FakeDataFactory.GeneratePostTags(1, 4);
            tags.AddRange(FakeDataFactory.GeneratePostTags(PostId, 4));
            tags.AddRange(FakeDataFactory.GeneratePostTags(3, 4));

            Insert("PostTag", tags);

            await _repository.DeleteByPostIdAsync(PostId);

            var currentTags = GetFromDatabase<PostTag>("SELECT * FROM PostTag");

            Assert.DoesNotContain(currentTags, x => x.PostId == PostId);
        }
    }
}
