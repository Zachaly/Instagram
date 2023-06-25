using Instagram.Application.Abstraction;
using Instagram.Database.Repository;
using Instagram.Domain.Entity;
using Instagram.Models.PostReport;
using Instagram.Models.PostReport.Request;
using Instagram.Models.Response;

namespace Instagram.Application
{
    public class PostReportService : ServiceBase<PostReport, PostReportModel, GetPostReportRequest, IPostReportRepository>, IPostReportService
    {
        private readonly IPostReportFactory _postReportFactory;
        private readonly IResponseFactory _responseFactory;

        public PostReportService(IPostReportRepository repository, IPostReportFactory postReportFactory, IResponseFactory responseFactory) : base(repository)
        {
            _postReportFactory = postReportFactory;
            _responseFactory = responseFactory;
        }

        public Task<ResponseModel> AddAsync(AddPostReportRequest request)
        {
            throw new NotImplementedException();
        }
    }
}
