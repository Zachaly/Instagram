using Instagram.Application.Abstraction;
using Instagram.Database.Repository;
using Instagram.Domain.Entity;
using Instagram.Models.PostReport;
using Instagram.Models.PostReport.Request;
using Instagram.Models.Response;

namespace Instagram.Application
{
    public class PostReportService : ServiceBase<PostReport, PostReportModel, GetPostReportRequest, AddPostReportRequest, IPostReportRepository>,
        IPostReportService
    {

        public PostReportService(IPostReportRepository repository, IPostReportFactory postReportFactory, IResponseFactory responseFactory)
            : base(repository, postReportFactory, responseFactory)
        {
        }
    }
}
