using Instagram.Application.Abstraction;
using Instagram.Domain.Entity;
using Instagram.Models.Relation.Request;

namespace Instagram.Application
{
    public class RelationFactory : IRelationFactory
    {
        public Relation Create(AddRelationRequest request)
        {
            throw new NotImplementedException();
        }

        public RelationImage CreateImage(long relationId, string fileName)
        {
            throw new NotImplementedException();
        }
    }
}
