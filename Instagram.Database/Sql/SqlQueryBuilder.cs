using Instagram.Domain.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Instagram.Database.Sql
{
    public class SqlQueryBuilder : ISqlQueryBuilder
    {
        public ISqlBuilderQuery BuildInsert<TEntity>(string table, TEntity entity) where TEntity : IEntity
        {
            throw new NotImplementedException();
        }

        public ISqlBuilderQuery BuildSelect<T>(string table)
        {
            throw new NotImplementedException();
        }
    }
}
