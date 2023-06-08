
using Instagram.Database.Repository;
using Instagram.Database.Sql;
using Instagram.Domain.Entity;

namespace Instagram.Tests.Integration.DatabaseTests
{
    public class PostImageRepositoryTests : RepositoryTest
    {
        private readonly PostImageRepository _repository;

        public PostImageRepositoryTests() : base()
        {
            _repository = new PostImageRepository(new SqlQueryBuilder(), _connectionFactory);
        }

        [Fact]
        public async Task DeleteByPostId_DeletesProperEntities()
        {
            const long IdToDelete = 2;

            var imagesToInsert = FakeDataFactory.GeneratePostImages(1, 10);
            imagesToInsert.AddRange(FakeDataFactory.GeneratePostImages(IdToDelete, 10));
            imagesToInsert.AddRange(FakeDataFactory.GeneratePostImages(3, 10));

            foreach(var image in imagesToInsert )
            {
                var query = new SqlQueryBuilder().BuildInsert("PostImage", image).Build();
                ExecuteQuery(query, image);
            }

            await _repository.DeleteByPostIdAsync(IdToDelete);

            var images = GetFromDatabase<PostImage>("SELECT * FROM PostImage");

            Assert.DoesNotContain(images, img => img.PostId == IdToDelete);
        }
    }
}
