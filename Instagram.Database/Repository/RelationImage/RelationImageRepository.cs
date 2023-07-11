using Instagram.Database.Factory;
using Instagram.Database.Repository.Abstraction;
using Instagram.Database.Sql;
using Instagram.Domain.Entity;
using Instagram.Models.RelationImage;
using Instagram.Models.RelationImage.Request;

namespace Instagram.Database.Repository
{
    public class RelationImageRepository : RepositoryBase<RelationImage, RelationImageModel, GetRelationImageRequest>, IRelationImageRepository
    {
        public RelationImageRepository(ISqlQueryBuilder sqlQueryBuilder, IConnectionFactory connectionFactory) : base(sqlQueryBuilder, connectionFactory)
        {
            Table = "RelationImage";
            DefaultOrderBy = "[RelationImage].[Id]";
        }

        public Task DeleteByRelationIdAsync(long relationId)
        {
            var param = new { RelationId = relationId };

            var query = _sqlQueryBuilder
                .BuildDelete(Table)
                .Where(param)
                .Build();

            return QueryAsync(query, param);
        }
    }
}
