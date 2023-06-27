using Instagram.Database.Repository.Abstraction;
using Instagram.Domain.Entity;
using Instagram.Models.PostReport;
using Instagram.Models.PostReport.Request;

namespace Instagram.Database.Repository
{
    public interface IPostReportRepository : IRepositoryBase<PostReport, PostReportModel, GetPostReportRequest>
    {
        Task UpdateByPostIdAsync(UpdatePostReportRequest request, long postId);
        Task UpdateByIdAsync(UpdatePostReportRequest request, long id);
    }
}
