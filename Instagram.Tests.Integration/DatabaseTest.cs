using Dapper;
using Instagram.Database.Sql;
using Instagram.Domain.Entity;
using System.Data.SqlClient;

namespace Instagram.Tests.Integration
{
    public abstract class DatabaseTest : IDisposable
    {
        protected IEnumerable<T> GetFromDatabase<T>(string query, object param = null)
        {
            using (var connection = new SqlConnection(Constants.ConnectionString))
            {
                return connection.Query<T>(query, param);
            }
        }

        protected void ExecuteQuery(string query, object param = null)
        {
            using (var connection = new SqlConnection(Constants.ConnectionString))
            {
                connection.Query(query, param);
            }
        }

        protected void Insert<T>(string table, T item) where T : IEntity
        {
            var query = new SqlQueryBuilder().BuildInsert(table, item).Build();

            ExecuteQuery(query, item);
        }

        protected void Insert<T>(string table, IEnumerable<T> items) where T : IEntity
        {
            foreach (var item in items)
            {
                var query = new SqlQueryBuilder().BuildInsert(table, item).Build();
                ExecuteQuery(query, item);
            }
        }

        public void Dispose()
        {
            using (var connection = new SqlConnection(Constants.ConnectionString))
            {
                foreach (var query in Constants.TruncateQueries)
                {
                    connection.Query(query);
                }
            }
        }
    }
}
