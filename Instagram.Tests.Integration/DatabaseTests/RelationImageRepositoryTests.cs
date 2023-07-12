using Instagram.Database.Repository;
using Instagram.Database.Sql;
using Instagram.Domain.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Instagram.Tests.Integration.DatabaseTests
{
    public class RelationImageRepositoryTests : RepositoryTest
    {
        private readonly RelationImageRepository _repository;

        public RelationImageRepositoryTests()
        {
            _repository = new RelationImageRepository(new SqlQueryBuilder(), _connectionFactory);
        }

        [Fact]
        public async Task DeleteByRelationId_RemovesProperEntities()
        {
            const long IdToDelete = 2;

            var imagesToInsert = new long[] { 1, IdToDelete, 3 }
                .SelectMany(id => FakeDataFactory.GenerateRelationImages(id, 4));

            Insert("RelationImage", imagesToInsert);

            await _repository.DeleteByRelationIdAsync(IdToDelete);

            var images = GetFromDatabase<RelationImage>("SELECT * FROM [RelationImage]");

            Assert.DoesNotContain(images, x => x.RelationId == IdToDelete);
        }
    }
}
