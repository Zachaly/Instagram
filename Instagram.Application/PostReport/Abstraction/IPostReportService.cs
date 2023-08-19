using Instagram.Models.PostReport;
using Instagram.Models.PostReport.Request;

namespace Instagram.Application.Abstraction
{
    public interface IPostReportService : IServiceBase<PostReportModel, GetPostReportRequest, AddPostReportRequest>
    {

    }
}
