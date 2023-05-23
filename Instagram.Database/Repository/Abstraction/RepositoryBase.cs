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

        public Task DeleteByIdAsync(long id)
        {
            throw new NotImplementedException();
        }

        public Task<TModel> GetByIdAsync(long id)
        {
            throw new NotImplementedException();
        }

        Task<long> IRepositoryBase<TEntity, TModel, TGetRequest>.InsertAsync(TEntity entity)
        {
            throw new NotImplementedException();
        }
    }
}
