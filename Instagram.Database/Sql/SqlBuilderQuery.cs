﻿using Instagram.Domain.SqlAttribute;
using Instagram.Models;
using System.Reflection;
using System.Text;

namespace Instagram.Database.Sql
{
    public class SqlBuilderQuery : ISqlBuilderQuery
    {
        private readonly StringBuilder _template;
        private readonly string _table;
        private readonly StringBuilder _where = new StringBuilder("");
        private readonly StringBuilder _orderBy = new StringBuilder("");
        private readonly StringBuilder _conditionalJoin = new StringBuilder("");
        private readonly StringBuilder _conditionalSelect = new StringBuilder("");
        private readonly StringBuilder _outerOrderBy = new StringBuilder("");
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
                .Replace("/**conditionaljoin**/", _conditionalJoin.ToString())
                .Replace("/**conditionalselect**/", _conditionalSelect.ToString())
                .Replace("/**outerOrderBy**/", _outerOrderBy.ToString())
                .ToString();
        }

        public ISqlBuilderQuery JoinConditional<TRequest>(TRequest request)
        {
            var joins = typeof(TRequest)
                .GetProperties()
                .Where(prop => prop.GetValue(request) != default)
                .Select(prop => prop.GetCustomAttribute<ConditionalJoinAttribute>())
                .Where(attr => attr is not null);

            var excludedJoins = new List<string>();

            foreach(var join in joins)
            {
                if (excludedJoins.Contains(join!.JoinedColumn))
                {
                    continue;
                }

                _conditionalJoin.Append($" LEFT OUTER JOIN [{join.Table}] ON {join.Condition}");
                if(!string.IsNullOrEmpty(join.JoinedColumn))
                {
                    excludedJoins.Add(join.JoinedColumn);

                    _conditionalSelect.Append(", ");
                    _conditionalSelect.Append(join.JoinedColumn);
                }
            }

            return this;
        }

        public ISqlBuilderQuery OrderBy(string orderBy)
        {
            if(_orderBy.ToString() == "")
            {
                _orderBy.Append("ORDER BY ");
            }
            _orderBy.Append(orderBy);

            return this;
        }

        public ISqlBuilderQuery OuterOrderBy(string orderBy)
        {
            if (_outerOrderBy.ToString() == "")
            {
                _outerOrderBy.Append("ORDER BY ");
            }
            _outerOrderBy.Append(orderBy);

            return this;
        }

        public ISqlBuilderQuery Where<TRequest>(TRequest request)
        {
            var props = typeof(TRequest).GetProperties()
                .Where(prop => prop.Name != "PageIndex" && prop.Name != "PageSize" && prop.Name != "SkipPagination")
                .Where(prop => prop.GetValue(request) is not null 
                && (prop.GetCustomAttribute<ConditionalJoinAttribute>() is null  || prop.GetCustomAttribute<WhereAttribute>() is not null) );

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
                var attribute = prop.GetCustomAttribute<WhereAttribute>();
                var name = "";

                if(attribute is null)
                {
                    name = $"[{_table}].[{prop.Name}] = @{prop.Name} ";
                } 
                else
                {
                    if(prop.PropertyType == typeof(string))
                    {
                        prop.SetValue(request, $"{attribute.Decorator}{prop.GetValue(request)}{attribute.Decorator}");
                    }
                    name = $"{attribute.Condition} @{prop.Name} ";
                }

                if(index == 0)
                {
                    where = name;
                }
                else
                {
                    where = $"AND {name}";
                }
                _where.Append(where);
                _where.Append(' ');
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
