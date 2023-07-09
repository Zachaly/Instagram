using Instagram.Database.Repository.Abstraction;
using Instagram.Domain.Entity;
using Instagram.Models.RelationImage;
using Instagram.Models.RelationImage.Request;

namespace Instagram.Database.Repository
{
    public interface IRelationImageRepository : IRepositoryBase<RelationImage, RelationImageModel, GetRelationImageRequest>
    {
        Task DeleteByRelationIdAsync(long relationId);
    }
}
