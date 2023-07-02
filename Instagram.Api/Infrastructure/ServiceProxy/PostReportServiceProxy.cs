using Instagram.Application.Abstraction;
using Instagram.Models.PostReport;
using Instagram.Models.PostReport.Request;
using Instagram.Models.Response;

namespace Instagram.Api.Infrastructure.ServiceProxy
{
    public interface IPostReportServiceProxy : IPostReportService { }

    public class PostReportServiceProxy : HttpLoggingServiceProxyBase<PostReportModel, GetPostReportRequest, IPostReportService>, IPostReportServiceProxy
    {
        public PostReportServiceProxy(ILogger<IPostReportService> logger, IHttpContextAccessor httpContextAccessor,
            IPostReportService postReportService) : base(logger, httpContextAccessor, postReportService)
        {
        }

        public async Task<ResponseModel> AddAsync(AddPostReportRequest request)
        {
            LogInformation("Add");

            var response = await _service.AddAsync(request);

            LogResponse(response, "Add");

            return response;
        }
    }
}
