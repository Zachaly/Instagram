using Instagram.Application.Abstraction;
using Instagram.Domain.Entity;
using Instagram.Models.Relation.Request;

namespace Instagram.Application
{
    public class RelationFactory : IRelationFactory
    {
        public Relation Create(AddRelationRequest request)
            => new Relation
            {
                Name = request.Name,
                UserId = request.UserId,
            };

        public RelationImage CreateImage(long relationId, string fileName)
            => new RelationImage
            {
                RelationId = relationId,
                FileName = fileName,
            };
    }
}
