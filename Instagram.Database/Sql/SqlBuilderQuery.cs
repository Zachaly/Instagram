using System.Text;

namespace Instagram.Database.Sql
{
    public class SqlBuilderQuery : ISqlBuilderQuery
    {
        private readonly StringBuilder _template;
        private readonly string _table;
        private readonly StringBuilder _where = new StringBuilder("");
        private readonly StringBuilder _orderBy = new StringBuilder("");

        public SqlBuilderQuery(string template, string table)
        {
            _template = new StringBuilder(template);
            _table = table;
        }

        public string Build()
        {
            return _template
                .Replace("/**where**/", _where.ToString())
                .Replace("/**orderby**/", _orderBy.ToString())
                .ToString();
        }

        public ISqlBuilderQuery OrderBy(string orderBy)
        {
            if(_orderBy.ToString() == "")
            {
                _orderBy.Append("ORDER BY");
            }
            _orderBy.Append($" [{_table}].[{orderBy}]");

            return this;
        }

        public ISqlBuilderQuery Where<TRequest>(TRequest request)
        {
            var props = typeof(TRequest).GetProperties().Where(prop => prop.GetValue(request) is not null).Select(prop => prop.Name);

            int index = 0;
            foreach(var prop in props)
            {
                string where = "";
                if(index == 0)
                {
                    _where.Append("WHERE ");
                    where = $"[{_table}].[{prop}] = @{prop} ";
                }
                else
                {
                    where = $"AND [{_table}].[{prop}] = @{prop} ";
                }
                _where.Append(where);
                index++;
            }
            return this;
        }
    }
}
