using Instagram.Database.Factory;
using Instagram.Database.Repository.Abstraction;
using Instagram.Database.Sql;
using Instagram.Domain.Entity;
using Instagram.Models.PostReport;
using Instagram.Models.PostReport.Request;

namespace Instagram.Database.Repository
{
    internal class PostReportRepository : RepositoryBase<PostReport, PostReportModel, GetPostReportRequest>, IPostReportRepository
    {
        public PostReportRepository(ISqlQueryBuilder sqlQueryBuilder, IConnectionFactory connectionFactory) : base(sqlQueryBuilder, connectionFactory)
        {
            Table = "PostReport";
            DefaultOrderBy = "[PostReport].[Created] DESC";
        }

        public Task UpdateByIdAsync(UpdatePostReportRequest request, long id)
        {
            throw new NotImplementedException();
        }

        public Task UpdateByPostIdAsync(UpdatePostReportRequest request, long postId)
        {
            throw new NotImplementedException();
        }
    }
}
