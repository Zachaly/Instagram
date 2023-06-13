using Instagram.Database.Repository.Abstraction;
using Instagram.Domain.Entity;
using Instagram.Models;

namespace Instagram.Application
{
    public interface IKeylessServiceBase<TModel, TGetRequest>
        where TModel : IModel
        where TGetRequest : PagedRequest
    {
        Task<IEnumerable<TModel>> GetAsync(TGetRequest request);
        Task<int> GetCountAsync(TGetRequest request);
    }

    public abstract class KeylessServiceBase<TEntity, TModel, TGetRequest, TRepository> : IKeylessServiceBase<TModel, TGetRequest>
        where TModel : IModel
        where TGetRequest : PagedRequest
        where TEntity : IEntity
        where TRepository : IKeylessRepositoryBase<TEntity, TModel, TGetRequest>
    {
        protected TRepository _repository;

        protected KeylessServiceBase(TRepository repository)
        {
            _repository = repository;
        }

        public Task<IEnumerable<TModel>> GetAsync(TGetRequest request)
        {
            return _repository.GetAsync(request);
        }

        public Task<int> GetCountAsync(TGetRequest request)
        {
            return _repository.GetCountAsync(request);
        }
    }
}
