using Instagram.Application;
using Instagram.Models;
using Instagram.Models.Response;

namespace Instagram.Api.Infrastructure.ServiceProxy
{
    public abstract class HttpLoggingProxyBase<TService>
    {
        private readonly ILogger<TService> _logger;
        private readonly IHttpContextAccessor _httpContextAccessor;

        protected HttpLoggingProxyBase(ILogger<TService> logger, IHttpContextAccessor httpContextAccessor)
        {
            _logger = logger;
            _httpContextAccessor = httpContextAccessor;
        }

        protected void LogInformation(string message = "")
        {
            var ip = _httpContextAccessor.HttpContext?.Connection.RemoteIpAddress?.ToString() ?? "Unknown";
            _logger.LogInformation("{Message} : {Time} : {IP}", message, DateTime.Now, ip);
        }

        protected void LogResponse(ResponseModel response, string message = "")
        {
            var ip = _httpContextAccessor.HttpContext?.Connection.RemoteIpAddress?.ToString() ?? "Unknown";

            if (response.Success)
            {
                _logger.LogInformation("{Message}: Success: {Time}: {IP}", message, DateTime.Now, ip);
                return;
            }

            _logger.LogWarning("{Message} - Error: {Error} : {Time}: {IP}", message, response.Error,
                    DateTime.Now, ip);
        }
    }

    public abstract class HttpLoggingKeylessServiceProxyBase<TModel, TGetRequest, TService> : HttpLoggingProxyBase<TService>, IKeylessServiceBase<TModel, TGetRequest>
        where TService : IKeylessServiceBase<TModel, TGetRequest>
        where TModel : IModel
        where TGetRequest : PagedRequest
    {
        protected readonly TService _service;

        protected HttpLoggingKeylessServiceProxyBase(ILogger<TService> logger, IHttpContextAccessor httpContextAccessor,
            TService service) : base(logger, httpContextAccessor)
        {
            _service = service;
        }

        public Task<IEnumerable<TModel>> GetAsync(TGetRequest request)
        {
            LogInformation("Get");

            return _service.GetAsync(request);
        }

        public Task<int> GetCountAsync(TGetRequest request)
        {
            LogInformation("Get Count");

            return _service.GetCountAsync(request);
        }
    }

    public abstract class HttpLoggingServiceProxyBase<TModel, TGetRequest, TService> : HttpLoggingKeylessServiceProxyBase<TModel, TGetRequest, TService>,
        IServiceBase<TModel, TGetRequest>
        where TService : IServiceBase<TModel, TGetRequest>
        where TModel : IModel
        where TGetRequest : PagedRequest
    {
        protected HttpLoggingServiceProxyBase(ILogger<TService> logger, IHttpContextAccessor httpContextAccessor, TService service) : base(logger, httpContextAccessor, service)
        {
        }

        public Task<TModel> GetByIdAsync(long id)
        {
            LogInformation("Get By Id");

            return _service.GetByIdAsync(id);
        }
    }
}
