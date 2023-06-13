using Instagram.Database.Repository.Abstraction;
using Instagram.Domain.Entity;
using Instagram.Models;

namespace Instagram.Application
{
    public interface IServiceBase<TModel, TGetRequest> : IKeylessServiceBase<TModel, TGetRequest>
        where TModel : IModel
        where TGetRequest : PagedRequest
    {
        Task<TModel> GetByIdAsync(long id);
    }

    public abstract class ServiceBase<TEntity, TModel, TGetRequest, TRepository> : KeylessServiceBase<TEntity, TModel, TGetRequest, TRepository>,
        IServiceBase<TModel, TGetRequest>
        where TModel : IModel
        where TGetRequest : PagedRequest
        where TEntity : IEntity
        where TRepository : IRepositoryBase<TEntity, TModel, TGetRequest>
    {
        protected ServiceBase(TRepository repository) : base(repository)
        {
        }

        public Task<TModel> GetByIdAsync(long id)
        {
            return _repository.GetByIdAsync(id);
        }
    }
}
