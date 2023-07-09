using Instagram.Database.Factory;
using Instagram.Database.Repository.Abstraction;
using Instagram.Database.Sql;
using Instagram.Domain.Entity;
using Instagram.Models.RelationImage;
using Instagram.Models.RelationImage.Request;

namespace Instagram.Database.Repository
{
    internal class RelationImageRepository : RepositoryBase<RelationImage, RelationImageModel, GetRelationImageRequest>, IRelationImageRepository
    {
        public RelationImageRepository(ISqlQueryBuilder sqlQueryBuilder, IConnectionFactory connectionFactory) : base(sqlQueryBuilder, connectionFactory)
        {
            Table = "RelationImage";
        }
    }
}
