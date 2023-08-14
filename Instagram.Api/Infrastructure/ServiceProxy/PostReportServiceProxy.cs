using FluentValidation;
using Instagram.Application.Abstraction;
using Instagram.Models.PostReport;
using Instagram.Models.PostReport.Request;
using Instagram.Models.Response;

namespace Instagram.Api.Infrastructure.ServiceProxy
{
    public interface IPostReportServiceProxy : IPostReportService { }

    public class PostReportServiceProxy : HttpLoggingServiceProxyBase<PostReportModel, GetPostReportRequest, IPostReportService>, IPostReportServiceProxy
    {
        private readonly IResponseFactory _responseFactory;
        private readonly IValidator<AddPostReportRequest> _addValidator;

        public PostReportServiceProxy(ILogger<IPostReportService> logger, IHttpContextAccessor httpContextAccessor,
            IPostReportService postReportService, IResponseFactory responseFactory, IValidator<AddPostReportRequest> addValidator)
            : base(logger, httpContextAccessor, postReportService)
        {
            _responseFactory = responseFactory;
            _addValidator = addValidator;
            ServiceName = "PostReport";
        }

        public async Task<ResponseModel> AddAsync(AddPostReportRequest request)
        {
            LogInformation("Add");

            var validation = _addValidator.Validate(request);

            var response = validation.IsValid ? await _service.AddAsync(request) : _responseFactory.CreateValidationError(validation);

            LogResponse(response, "Add");

            return response;
        }
    }
}
