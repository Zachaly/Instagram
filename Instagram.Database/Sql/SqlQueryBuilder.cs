using Instagram.Domain.Entity;
using Instagram.Domain.SqlAttribute;
using System.Reflection;
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
            var select = new StringBuilder($"SELECT /**select**//**conditionalselect**/ FROM [{table}] /**join**/ /**conditionaljoin**/ /**where**/ /**orderby**/ /**pagination**/");

            var joins = typeof(T).GetCustomAttributes<JoinAttribute>();

            var joinBuilder = new StringBuilder("");

            if (joins.Any(join => join.OutsideJoin))
            {
                return BuildSelectWithSubQuery<T>(table);
            }

            foreach (var join in joins)
            {
                joinBuilder.Append($"LEFT OUTER JOIN [{join.Table}] ON {join.Condition}");
            }

            var selectedValues = SelectValues<T>(table);

            select.Replace("/**select**/", selectedValues.ToString());
            select.Replace("/**join**/", joinBuilder.ToString());

            return new SqlBuilderQuery(select.ToString(), table);
        }

        private ISqlBuilderQuery BuildSelectWithSubQuery<T>(string table)
        {
            var select = new StringBuilder($"SELECT t.* /**outsideselect**/ FROM (SELECT /**select**/ FROM [{table}] /**subjoin**/ /**where**/ /**orderby**/ /**pagination**/) as t /**join**/ /**groupby**/");

            var innerSelect = SelectValues<T>(table);
            var outerSelect = SelectOuterQueryValues<T>();

            var joins = typeof(T).GetCustomAttributes<JoinAttribute>();

            var subJoinBuilder = new StringBuilder("");
            var outsideJoinBuilder = new StringBuilder("");

            foreach (var join in joins)
            {
                if(join.OutsideJoin)
                {
                    outsideJoinBuilder.Append($"LEFT OUTER JOIN [{join.Table}] ON {join.Condition}");
                } 
                else
                {
                    subJoinBuilder.Append($"LEFT OUTER JOIN [{join.Table}] ON {join.Condition}");
                }
            }

            select.Replace("/**select**/", innerSelect.ToString());
            select.Replace("/**outsideselect**/", outerSelect.ToString());
            select.Replace("/**join**/", outsideJoinBuilder.ToString());
            select.Replace("/**subjoin**/", subJoinBuilder.ToString());
            select.Replace("/**groupby**/", GenerateGroupBy<T>());

            return new SqlBuilderQuery(select.ToString(), table);
        }

        private string GenerateGroupBy<T>()
        {
            if(typeof(T).GetCustomAttribute<GroupByAttribute>() is null)
            {
                return "";
            }

            var groupBuilder = new StringBuilder("GROUP BY ");

            var props = typeof(T)
                .GetProperties()
                .Where(prop => prop.GetCustomAttribute<NotGroupedAttribute>() is null);

            var index = 0;

            foreach (var prop in props)
            {
                var sqlName = prop.GetCustomAttribute<SqlNameAttribute>();
                var coma = " ";
                var name = $"t.{prop.Name}";

                if(index != 0)
                {
                    coma = ", ";
                }

                if(sqlName?.OuterQuery ?? false)
                {
                    name = sqlName.Name;
                }

                groupBuilder.Append($"{coma} {name}");

                index++;
            }

            return groupBuilder.ToString();
        }

        private StringBuilder SelectValues<T>(string table)
        {
            var selectedValues = new StringBuilder("");
            int index = 0;
            var props = typeof(T).GetProperties();

            foreach (var prop in props)
            {
                var coma = "";
                var name = $"[{table}].[{prop.Name}] as [{prop.Name}]";
                if (index != 0)
                {
                    coma = ",";
                }
                var sqlName = prop.GetCustomAttribute<SqlNameAttribute>();
                if (sqlName is not null)
                {
                    if (sqlName.Conditional || sqlName.OuterQuery)
                    {
                        continue;
                    }
                    name = $"{sqlName.Name} as {prop.Name}";
                }
                selectedValues.Append($"{coma} {name}");
                index++;
            }

            return selectedValues;
        }

        private StringBuilder SelectOuterQueryValues<T>()
        {
            var selectedValues = new StringBuilder("");

            var props = typeof(T)
                .GetProperties()
                .Select(prop => new { Attr = prop.GetCustomAttribute<SqlNameAttribute>(), PropName = prop.Name } )
                .Where(prop => prop.Attr is not null && prop.Attr.OuterQuery);

            foreach (var prop in props)
            {
                selectedValues.Append($", {prop.Attr.Name}");
            }

            return selectedValues;
        }

        public ISqlBuilderQuery BuildUpdate<TRequest>(string table, TRequest request)
        {
            var update = new StringBuilder($"UPDATE [{table}] /**set**/ /**where**/");

            var props = typeof(TRequest)
                .GetProperties()
                .Where(prop => prop.GetValue(request) is not null && prop.Name != "Id")
                .Select(prop => prop.Name);

            var columns = new StringBuilder("");

            int index = 0;
            foreach (var prop in props)
            {
                var coma = "";
                if (index != 0)
                {
                    coma = ",";
                } else
                {
                    columns.Append("SET ");
                }
                columns.Append($"{coma} [{table}].[{prop}]=@{prop}");
                index++;
            }

            update.Replace("/**set**/", columns.ToString());

            return new SqlBuilderQuery(update.ToString(), table);
        }

        public ISqlBuilderQuery BuildCount(string table)
        {
            var template = $"SELECT COUNT(*) FROM [{table}] /**where**/";

            return new SqlBuilderQuery(template, table);
        }
    }
}
