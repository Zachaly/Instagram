using Dapper;
using Instagram.Database.Factory;
using Instagram.Database.Sql;
using Instagram.Domain.Entity;
using Instagram.Models;

namespace Instagram.Database.Repository.Abstraction
{
    public abstract class KeylessRepositoryBase<TEntity, TModel, TGetRequest> : IKeylessRepositoryBase<TEntity, TModel, TGetRequest>
        where TEntity : IEntity
        where TModel : IModel
        where TGetRequest : PagedRequest
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

        public virtual Task<IEnumerable<TModel>> GetAsync(TGetRequest request)
        {
            var query = _sqlQueryBuilder
                .BuildSelect<TModel>(Table)
                .Where(request)
                .OrderBy(DefaultOrderBy)
                .WithPagination(request)
                .Build();

            return QueryManyAsync<TModel>(query, request);
        }

        public virtual Task InsertAsync(TEntity entity)
        {
            var query = _sqlQueryBuilder
                .BuildInsert(Table, entity)
                .Build();

            return QueryAsync(query, entity);
        }

        protected async Task<T> QuerySingleAsync<T>(string query, object param = null)
        {
            using(var connection = _connectionFactory.CreateConnection())
            {
                return await connection.QuerySingleOrDefaultAsync<T>(query, param);
            }
        }

        protected async Task<IEnumerable<T>> QueryManyAsync<T>(string query, object param = null)
        {
            using(var connection = _connectionFactory.CreateConnection())
            {
                return await connection.QueryAsync<T>(query, param);
            }
        }

        protected async Task QueryAsync(string query, object param = null)
        {
            using(var connection = _connectionFactory.CreateConnection())
            {
                await connection.QueryAsync(query, param);
            }
        }

        public Task<int> GetCount(TGetRequest request)
        {
            throw new NotImplementedException();
        }
    }
}
