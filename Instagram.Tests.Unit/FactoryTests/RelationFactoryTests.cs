using Instagram.Application;
using Instagram.Models.Relation.Request;

namespace Instagram.Tests.Unit.FactoryTests
{
    public class RelationFactoryTests
    {
        private readonly RelationFactory _factory;

        public RelationFactoryTests()
        {
            _factory = new RelationFactory();
        }

        [Fact]
        public void Create_CreatesValidEntity()
        {
            var request = new AddRelationRequest
            {
                Name = "name",
                UserId = 1,
            };

            var relation = _factory.Create(request);

            Assert.Equal(request.Name, relation.Name);
            Assert.Equal(request.UserId, relation.UserId);
        }

        [Fact]
        public void CreateImage_CreatesValidEntity()
        {
            const long RelationId = 1;
            const string FileName = "file";

            var image = _factory.CreateImage(RelationId, FileName);

            Assert.Equal(RelationId, image.RelationId);
            Assert.Equal(FileName, image.FileName);
        }
    }
}
