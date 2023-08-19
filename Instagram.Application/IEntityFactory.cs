using Instagram.Domain.Entity;

namespace Instagram.Application
{
    public interface IEntityFactory<TEntity, TAddRequest>
        where TEntity : IEntity
    {
        TEntity Create(TAddRequest request);
    }
}
