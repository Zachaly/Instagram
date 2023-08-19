using Instagram.Application.Abstraction;
using Instagram.Database.Repository.Abstraction;
using Instagram.Domain.Entity;
using Instagram.Models;
using Instagram.Models.Response;

namespace Instagram.Application
{
    public interface IKeylessServiceBase<TModel, TGetRequest>
        where TModel : IModel
        where TGetRequest : PagedRequest
    {
        Task<IEnumerable<TModel>> GetAsync(TGetRequest request);
        Task<int> GetCountAsync(TGetRequest request);
    }

    public interface IKeylessServiceBase<TModel, TGetRequest, TAddRequest> : IKeylessServiceBase<TModel, TGetRequest>
        where TModel : IModel
        where TGetRequest : PagedRequest
    {
        Task<ResponseModel> AddAsync(TAddRequest request);
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

    public abstract class KeylessServiceBase<TEntity, TModel, TGetRequest, TAddRequest, TRepository>
        : KeylessServiceBase<TEntity, TModel, TGetRequest, TRepository>, IKeylessServiceBase<TModel, TGetRequest, TAddRequest>
        where TModel : IModel
        where TGetRequest : PagedRequest
        where TEntity : IEntity
        where TRepository : IKeylessRepositoryBase<TEntity, TModel, TGetRequest>
    {
        protected readonly IEntityFactory<TEntity, TAddRequest> _entityFactory;
        protected readonly IResponseFactory _responseFactory;

        protected KeylessServiceBase(TRepository repository,
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

                await _repository.InsertAsync(entity);

                return _responseFactory.CreateSuccess();
            }
            catch (Exception ex)
            {
                return _responseFactory.CreateFailure(ex.Message);
            }
        }
    }
}
