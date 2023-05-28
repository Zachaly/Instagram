using Instagram.Domain.Entity;

namespace Instagram.Database.Sql
{
    public interface ISqlQueryBuilder
    {
        ISqlBuilderQuery BuildSelect<T>(string table);
        ISqlBuilderQuery BuildInsert<TEntity>(string table, TEntity entity) where TEntity : IEntity;
        ISqlBuilderQuery BuildDelete(string table);
        ISqlBuilderQuery BuildUpdate<TRequest>(string table, TRequest request);
    }
}
