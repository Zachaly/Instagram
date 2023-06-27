using Instagram.Application.Abstraction;
using Instagram.Models.PostReport;
using Instagram.Models.PostReport.Request;
using Instagram.Models.Response;

namespace Instagram.Api.Infrastructure.ServiceProxy
{
    public interface IPostReportServiceProxy : IPostReportService { }

    public class PostReportServiceProxy : HttpLoggingProxyBase<IPostReportService>, IPostReportServiceProxy
    {
        private readonly IPostReportService _postReportService;

        public PostReportServiceProxy(ILogger<IPostReportService> logger, IHttpContextAccessor httpContextAccessor,
            IPostReportService postReportService) : base(logger, httpContextAccessor)
        {
            _postReportService = postReportService;
        }

        public async Task<ResponseModel> AddAsync(AddPostReportRequest request)
        {
            LogInformation("Add");

            var response = await _postReportService.AddAsync(request);

            LogResponse(response, "Add");

            return response;
        }

        public Task<IEnumerable<PostReportModel>> GetAsync(GetPostReportRequest request)
        {
            LogInformation("Get");

            return _postReportService.GetAsync(request);
        }

        public Task<PostReportModel> GetByIdAsync(long id)
        {
            LogInformation("Get By Id");

            return _postReportService.GetByIdAsync(id);
        }

        public Task<int> GetCountAsync(GetPostReportRequest request)
        {
            LogInformation("Get Count");

            return _postReportService.GetCountAsync(request);
        }
    }
}
