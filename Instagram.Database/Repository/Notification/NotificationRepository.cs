using Instagram.Database.Factory;
using Instagram.Database.Repository.Abstraction;
using Instagram.Database.Sql;
using Instagram.Domain.Entity;
using Instagram.Models.Notification;
using Instagram.Models.Notification.Request;

namespace Instagram.Database.Repository
{
    public class NotificationRepository : RepositoryBase<Notification, NotificationModel, GetNotificationRequest>, INotificationRepository
    {
        public NotificationRepository(ISqlQueryBuilder sqlQueryBuilder, IConnectionFactory connectionFactory) : base(sqlQueryBuilder, connectionFactory)
        {
            Table = "Notification";
            DefaultOrderBy = "[Notification].[Created] DESC";
        }

        public Task UpdateAsync(UpdateNotificationRequest request)
        {
            var query = _sqlQueryBuilder
                .BuildUpdate(Table, request)
                .Where(new { Id = request.Id })
                .Build();

            return QueryAsync(query, request);
        }
    }
}
