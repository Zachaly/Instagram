using Instagram.Models;

namespace Instagram.Database.Sql
{
    public interface ISqlBuilderQuery
    {
        string Build();
        ISqlBuilderQuery Where<TRequest>(TRequest request);
        ISqlBuilderQuery OrderBy(string orderBy);
        ISqlBuilderQuery WithPagination(PagedRequest request);
        ISqlBuilderQuery JoinConditional<TRequest>(TRequest request);
    }
}
