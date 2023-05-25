using Instagram.Domain.Entity;
using System.Text;

namespace Instagram.Database.Sql
{
    public class SqlQueryBuilder : ISqlQueryBuilder
    {
        public ISqlBuilderQuery BuildDelete(string table)
        {
            var template = $"DELETE FROM [{table}] /**where**/";

            return new SqlBuilderQuery(template, table);
        }

        public ISqlBuilderQuery BuildInsert<TEntity>(string table, TEntity entity) where TEntity : IEntity
        {
            var insert = new StringBuilder($"INSERT INTO [{table}](/**columns**/) /**output**/ VALUES(/**values**/)");


            var properties = typeof(TEntity).GetProperties();
            var output = properties.Any(p => p.Name == "Id") ? "OUTPUT INSERTED.[Id]" : "";

            var columns = string.Join(", ", properties.Where(p => p.Name != "Id").Select(p => $"[{p.Name}]"));
            var values = string.Join(", ", properties.Where(p => p.Name != "Id").Select(p => $"@{p.Name}"));

            var template = insert
                .Replace("/**columns**/", columns)
                .Replace("/**values**/", values)
                .Replace("/**output**/", output)
                .ToString();

            return new SqlBuilderQuery(template, table);
        }

        public ISqlBuilderQuery BuildSelect<T>(string table)
        {
            var select = new StringBuilder($"SELECT /**select**/ FROM [{table}] /**where**/ /**orderby**/");

            var props = typeof(T).GetProperties().Select(prop => prop.Name);

            var selectedValues = new StringBuilder("");

            int index = 0;
            foreach(var prop in props)
            {
                var coma = "";
                if(index != 0)
                {
                    coma = ",";
                }
                selectedValues.Append($"{coma} [{table}].[{prop}]");
                index++;
            }

            select.Replace("/**select**/", selectedValues.ToString());

            return new SqlBuilderQuery(select.ToString(), table);
        }
    }
}
