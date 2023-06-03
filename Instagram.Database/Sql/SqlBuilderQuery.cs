using Instagram.Models;
using System.Text;

namespace Instagram.Database.Sql
{
    public class SqlBuilderQuery : ISqlBuilderQuery
    {
        private readonly StringBuilder _template;
        private readonly string _table;
        private readonly StringBuilder _where = new StringBuilder("");
        private readonly StringBuilder _orderBy = new StringBuilder("");
        private string _pagination = "";

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
                .Replace("/**pagination**/", _pagination)
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
            var props = typeof(TRequest).GetProperties()
                .Where(prop => prop.Name != "PageIndex" && prop.Name != "PageSize" && prop.Name != "SkipPagination")
                .Where(prop => prop.GetValue(request) is not null).Select(prop => prop.Name);

            int index = 0;

            if (!props.Any())
            {
                return this;
            }

            if (!_where.ToString().Contains("WHERE"))
            {
                _where.Append("WHERE ");
            }

            foreach(var prop in props)
            {
                string where = "";
                if(index == 0)
                {
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

        public ISqlBuilderQuery WithPagination(PagedRequest request)
        {
            if (request.SkipPagination.GetValueOrDefault())
            {
                return this;
            }

            var index = request.PageIndex ?? 0;
            var pageSize = request.PageSize ?? 10;

            _pagination = $"OFFSET {index * pageSize} ROWS FETCH NEXT {pageSize} ROWS ONLY";

            return this;
        }
    }
}
