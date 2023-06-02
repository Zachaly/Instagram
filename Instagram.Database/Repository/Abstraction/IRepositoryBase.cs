using Instagram.Domain.Entity;
using Instagram.Models;

namespace Instagram.Database.Repository.Abstraction
{
    public interface IRepositoryBase<TEntity, TModel, TGetRequest> : IKeylessRepositoryBase<TEntity, TModel, TGetRequest>
        where TEntity : IEntity
        where TModel : IModel
        where TGetRequest : PagedRequest
    {
        new Task<long> InsertAsync(TEntity entity);
        Task<TModel> GetByIdAsync(long id);
        Task DeleteByIdAsync(long id);
        Task<TEntity> GetEntityByIdAsync(long id);
    }
}
