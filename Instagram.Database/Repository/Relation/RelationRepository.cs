using Dapper;
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
            DefaultOrderBy = "[Relation].[Id]";
        }

        public override async Task<IEnumerable<RelationModel>> GetAsync(GetRelationRequest request)
        {
            var query = _sqlQueryBuilder
                .BuildSelect<RelationModel>(Table)
                .Where(request)
                .WithPagination(request)
                .OrderBy(DefaultOrderBy)
                .Build();

            var lookup = new Dictionary<long, RelationModel>();
            using(var connection = _connectionFactory.CreateConnection())
            {
                await connection.QueryAsync<RelationModel, long, RelationModel>(query, (relation, imageId) =>
                {
                    RelationModel model;

                    if(!lookup.TryGetValue(relation.Id, out model))
                    {
                        model = relation;
                        lookup.Add(relation.Id, model);
                    }

                    model.ImageIds ??= new List<long>();

                    if (!model.ImageIds.Contains(imageId))
                    {
                        (model.ImageIds as List<long>)!.Add(imageId);
                    }
                    
                    return relation;
                }, request, splitOn: "Id, Id");
            }

            return lookup.Values;
        }

        public override async Task<RelationModel> GetByIdAsync(long id)
        {
            var param = new { Id = id };

            var query = _sqlQueryBuilder
                .BuildSelect<RelationModel>(Table)
                .Where(param)
                .Build();

            RelationModel model = null;
            using (var connection = _connectionFactory.CreateConnection())
            {
                await connection.QueryAsync<RelationModel, long, RelationModel>(query, (relation, imageId) =>
                {
                    model = model ?? relation;

                    model.ImageIds ??= new List<long>();

                    if (!model.ImageIds.Contains(imageId))
                    {
                        (model.ImageIds as List<long>)!.Add(imageId);
                    }

                    return relation;
                }, param, splitOn: "Id, Id");
            }

            return model;
        }
    }
}
