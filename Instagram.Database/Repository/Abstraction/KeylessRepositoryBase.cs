using Instagram.Database.Factory;
using Instagram.Database.Sql;
using Instagram.Domain.Entity;
using Instagram.Models;

namespace Instagram.Database.Repository.Abstraction
{
    public abstract class KeylessRepositoryBase<TEntity, TModel, TGetRequest> : IKeylessRepositoryBase<TEntity, TModel, TGetRequest>
        where TEntity : IEntity
        where TModel : IModel
    {
        protected readonly ISqlQueryBuilder _sqlQueryBuilder;
        protected readonly IConnectionFactory _connectionFactory;
        protected string Table { get; set; }
        protected string DefaultOrderBy { get; set; }


        protected KeylessRepositoryBase(ISqlQueryBuilder sqlQueryBuilder, IConnectionFactory connectionFactory)
        {
            _sqlQueryBuilder = sqlQueryBuilder;
            _connectionFactory = connectionFactory;
        }

        public Task<IEnumerable<TModel>> GetAsync(TGetRequest request)
        {
            throw new NotImplementedException();
        }

        public Task InsertAsync(TEntity entity)
        {
            throw new NotImplementedException();
        }
    }
}
