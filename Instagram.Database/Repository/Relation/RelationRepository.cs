using Instagram.Database.Factory;
using Instagram.Database.Repository.Abstraction;
using Instagram.Database.Sql;
using Instagram.Domain.Entity;
using Instagram.Models.Relation;
using Instagram.Models.Relation.Request;

namespace Instagram.Database.Repository
{
    public class RelationRepository : RepositoryBase<Relation, RelationModel, GetRelationRequest>, IRelationRepository
    {
        public RelationRepository(ISqlQueryBuilder sqlQueryBuilder, IConnectionFactory connectionFactory) : base(sqlQueryBuilder, connectionFactory)
        {
            Table = "Relation";
        }

        public override Task<IEnumerable<RelationModel>> GetAsync(GetRelationRequest request)
        {
            return base.GetAsync(request);
        }

        public override Task<RelationModel> GetByIdAsync(long id)
        {
            return base.GetByIdAsync(id);
        }
    }
}
