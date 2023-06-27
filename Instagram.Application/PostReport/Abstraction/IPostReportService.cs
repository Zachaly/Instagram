using Instagram.Models.PostReport;
using Instagram.Models.PostReport.Request;
using Instagram.Models.Response;

namespace Instagram.Application.Abstraction
{
    public interface IPostReportService : IServiceBase<PostReportModel, GetPostReportRequest>
    {
        Task<ResponseModel> AddAsync(AddPostReportRequest request);
    }
}
