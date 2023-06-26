using Dapper;
using Instagram.Database.Factory;
using Instagram.Database.Repository.Abstraction;
using Instagram.Database.Sql;
using Instagram.Domain.Entity;
using Instagram.Models.PostReport;
using Instagram.Models.PostReport.Request;

namespace Instagram.Database.Repository
{
    public class PostReportRepository : RepositoryBase<PostReport, PostReportModel, GetPostReportRequest>, IPostReportRepository
    {
        public PostReportRepository(ISqlQueryBuilder sqlQueryBuilder, IConnectionFactory connectionFactory) : base(sqlQueryBuilder, connectionFactory)
        {
            Table = "PostReport";
            DefaultOrderBy = "[PostReport].[Created] DESC";
        }

        public Task UpdateByIdAsync(UpdatePostReportRequest request, long id)
        {
            var query = _sqlQueryBuilder
                .BuildUpdate(Table, request)
                .Where(new { Id = id })
                .Build();

            request.Id = id;
            
            return QueryAsync(query, request);
        }

        public Task UpdateByPostIdAsync(UpdatePostReportRequest request, long postId)
        {
            var query = _sqlQueryBuilder
                .BuildUpdate(Table, request)
                .Where(new { PostId = postId })
                .Build();

            request.PostId = postId;

            return QueryAsync(query, request);
        }
    }
}
