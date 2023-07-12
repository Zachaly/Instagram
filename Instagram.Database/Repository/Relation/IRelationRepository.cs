using Instagram.Database.Repository.Abstraction;
using Instagram.Domain.Entity;
using Instagram.Models.Relation;
using Instagram.Models.Relation.Request;

namespace Instagram.Database.Repository
{
    public interface IRelationRepository : IRepositoryBase<Relation, RelationModel, GetRelationRequest>
    {
    }
}
