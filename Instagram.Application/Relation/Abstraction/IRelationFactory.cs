using Instagram.Domain.Entity;
using Instagram.Models.Relation.Request;

namespace Instagram.Application.Abstraction
{
    public interface IRelationFactory
    {
        Relation Create(AddRelationRequest request);
        RelationImage CreateImage(long relationId, string fileName);
    }
}
