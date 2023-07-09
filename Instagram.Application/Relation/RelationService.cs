using Instagram.Application.Abstraction;
using Instagram.Database.Repository;
using Instagram.Domain.Entity;
using Instagram.Models.Relation;
using Instagram.Models.Relation.Request;

namespace Instagram.Application
{
    public class RelationService : ServiceBase<Relation, RelationModel, GetRelationRequest, IRelationRepository>, IRelationService
    {
        public RelationService(IRelationRepository repository) : base(repository)
        {
        }
    }
}
