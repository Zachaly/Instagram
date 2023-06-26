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

        public async Task<ResponseModel> AddAsync(AddPostReportRequest request)
        {
            try
            {
                var report = _postReportFactory.Create(request);

                var id = await _repository.InsertAsync(report);

                return _responseFactory.CreateSuccess(id);
            }
            catch(Exception ex)
            {
                return _responseFactory.CreateFailure(ex.Message);
            }
        }
    }
}
