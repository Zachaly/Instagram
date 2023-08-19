using FluentValidation;
using Instagram.Api.Infrastructure.ServiceProxy.Abstraction;
using Instagram.Application.Abstraction;
using Instagram.Models.PostReport;
using Instagram.Models.PostReport.Request;
using Instagram.Models.Response;

namespace Instagram.Api.Infrastructure.ServiceProxy
{
    public interface IPostReportServiceProxy : IPostReportService { }

    public class PostReportServiceProxy 
        : HttpLoggingServiceProxyBase<PostReportModel, GetPostReportRequest, AddPostReportRequest, IPostReportService>,
        IPostReportServiceProxy
    {
        public PostReportServiceProxy(ILogger<IPostReportService> logger, IHttpContextAccessor httpContextAccessor,
            IPostReportService postReportService, IResponseFactory responseFactory, IValidator<AddPostReportRequest> addValidator)
            : base(logger, httpContextAccessor, postReportService, responseFactory, addValidator)
        {
            ServiceName = "PostReport";
        }
    }
}
