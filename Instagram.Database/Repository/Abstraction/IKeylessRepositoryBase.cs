using Instagram.Domain.Entity;
using Instagram.Models;

namespace Instagram.Database.Repository.Abstraction
{
    public interface IKeylessRepositoryBase<TEntity, TModel, TGetRequest> 
        where TEntity : IEntity
        where TModel : IModel
        where TGetRequest : PagedRequest
    {
        Task InsertAsync(TEntity entity);
        Task<IEnumerable<TModel>> GetAsync(TGetRequest request);
        Task<int> GetCount(TGetRequest request);
    }
}
