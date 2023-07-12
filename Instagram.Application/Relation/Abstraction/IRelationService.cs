using Instagram.Models.Relation;
using Instagram.Models.Relation.Request;

namespace Instagram.Application.Abstraction
{
    public interface IRelationService : IServiceBase<RelationModel, GetRelationRequest>
    {
    }
}
