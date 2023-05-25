namespace Instagram.Database.Sql
{
    public interface ISqlBuilderQuery
    {
        string Build();
        ISqlBuilderQuery Where<TRequest>(TRequest request);
        ISqlBuilderQuery OrderBy(string orderBy);
    }
}
