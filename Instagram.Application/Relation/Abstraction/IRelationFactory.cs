using Instagram.Domain.Entity;
using Instagram.Models.Relation.Request;

namespace Instagram.Application.Abstraction
{
    public interface IRelationFactory : IEntityFactory<Relation, AddRelationRequest>
    {
        RelationImage CreateImage(long relationId, string fileName);
    }
}
