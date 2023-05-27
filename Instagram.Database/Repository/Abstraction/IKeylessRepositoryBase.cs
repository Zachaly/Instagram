using Instagram.Domain.Entity;
using Instagram.Models;

namespace Instagram.Database.Repository.Abstraction
{
    public interface IKeylessRepositoryBase<TEntity, TModel, TGetRequest> 
        where TEntity : IEntity
        where TModel : IModel
    {
        Task InsertAsync(TEntity entity);
        Task<IEnumerable<TModel>> GetAsync(TGetRequest request);
    }
}
