using Instagram.Database.Factory;
using Instagram.Database.Sql;
using Instagram.Domain.Entity;
using Instagram.Models;

namespace Instagram.Database.Repository.Abstraction
{
    public abstract class RepositoryBase<TEntity, TModel, TGetRequest> : KeylessRepositoryBase<TEntity, TModel, TGetRequest>, IRepositoryBase<TEntity, TModel, TGetRequest>
        where TEntity : IEntity
        where TModel : IModel
    {
        protected RepositoryBase(ISqlQueryBuilder sqlQueryBuilder, IConnectionFactory connectionFactory) : base(sqlQueryBuilder, connectionFactory)
        {
        }

        public virtual Task DeleteByIdAsync(long id)
        {
            var param = new { Id = id };
            var query = _sqlQueryBuilder
                .BuildDelete(Table)
                .Where(param)
                .Build();

            return QueryAsync(query, param);
        }

        public virtual Task<TModel> GetByIdAsync(long id)
        {
            var param = new { Id = id };
            var query = _sqlQueryBuilder
                .BuildSelect<TModel>(Table)
                .Where(param)
                .Build();

            return QuerySingleAsync<TModel>(query, param);
        }

        public Task<TEntity> GetEntityByIdAsync(long id)
        {
            var param = new { Id = id };
            var query = _sqlQueryBuilder
                .BuildSelect<TEntity>(Table)
                .Where(param)
                .Build();

            return QuerySingleAsync<TEntity>(query, param);
        }

        public override Task<long> InsertAsync(TEntity entity)
        {
            var query = _sqlQueryBuilder
                .BuildInsert(Table, entity)
                .Build();

            return QuerySingleAsync<long>(query, entity);
        }
    }
}
