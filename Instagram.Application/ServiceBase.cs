using Instagram.Application.Abstraction;
using Instagram.Database.Repository.Abstraction;
using Instagram.Domain.Entity;
using Instagram.Models;
using Instagram.Models.Response;

namespace Instagram.Application
{
    public interface IServiceBase<TModel, TGetRequest> : IKeylessServiceBase<TModel, TGetRequest>
        where TModel : IModel
        where TGetRequest : PagedRequest
    {
        Task<TModel> GetByIdAsync(long id);
    }

    public interface IServiceBase<TModel, TGetRequest, TAddRequest> : IServiceBase<TModel, TGetRequest>
        where TModel : IModel
        where TGetRequest : PagedRequest
    {
        Task<ResponseModel> AddAsync(TAddRequest request);
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

    public abstract class ServiceBase<TEntity, TModel, TGetRequest, TAddRequest, TRepository> : ServiceBase<TEntity, TModel, TGetRequest, TRepository>,
        IServiceBase<TModel, TGetRequest, TAddRequest>
        where TModel : IModel
        where TGetRequest : PagedRequest
        where TEntity : IEntity
        where TRepository : IRepositoryBase<TEntity, TModel, TGetRequest>
    {
        protected readonly IResponseFactory _responseFactory;
        protected readonly IEntityFactory<TEntity, TAddRequest> _entityFactory;

        protected ServiceBase(TRepository repository,
            IEntityFactory<TEntity, TAddRequest> entityFactory,
            IResponseFactory responseFactory) : base(repository)
        {
            _entityFactory = entityFactory;
            _responseFactory = responseFactory;
        }

        public async Task<ResponseModel> AddAsync(TAddRequest request)
        {
            try
            {
                var entity = _entityFactory.Create(request);

                var newId = await _repository.InsertAsync(entity);

                return _responseFactory.CreateSuccess(newId);
            }
            catch(Exception ex)
            {
                return _responseFactory.CreateFailure(ex.Message);
            }
        }
    }
}
