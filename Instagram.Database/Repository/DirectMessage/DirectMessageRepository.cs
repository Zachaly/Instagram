using Instagram.Database.Factory;
using Instagram.Database.Repository.Abstraction;
using Instagram.Database.Sql;
using Instagram.Domain.Entity;
using Instagram.Models.DirectMessage;
using Instagram.Models.DirectMessage.Request;

namespace Instagram.Database.Repository
{
    public class DirectMessageRepository : RepositoryBase<DirectMessage, DirectMessageModel, GetDirectMessageRequest>, IDirectMessageRepository
    {
        public DirectMessageRepository(ISqlQueryBuilder sqlQueryBuilder, IConnectionFactory connectionFactory) : base(sqlQueryBuilder, connectionFactory)
        {
            Table = "DirectMessage";
            DefaultOrderBy = "[DirectMessage].[Created] DESC";
        }

        public Task UpdateAsync(UpdateDirectMessageRequest request)
        {
            var query = _sqlQueryBuilder
                .BuildUpdate(Table, request)
                .Where(new { Id = request.Id })
                .Build();

            return QueryAsync(query, request);
        }
    }
}
