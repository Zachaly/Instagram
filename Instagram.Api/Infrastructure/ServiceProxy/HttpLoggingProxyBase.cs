using Instagram.Application;
using Instagram.Models;
using Instagram.Models.Response;
using Serilog.Context;
using System.Security.Claims;

namespace Instagram.Api.Infrastructure.ServiceProxy
{
    public abstract class HttpLoggingProxyBase<TService>
    {
        private readonly ILogger<TService> _logger;
        private readonly IHttpContextAccessor _httpContextAccessor;
        protected string ServiceName { get; set; }

        protected HttpLoggingProxyBase(ILogger<TService> logger, IHttpContextAccessor httpContextAccessor)
        {
            _logger = logger;
            _httpContextAccessor = httpContextAccessor;
        }

        protected void LogInformation(string message = "")
        {
            var ip = GetRequestIpAddress();
            var userId = GetRequestUserId();

            using (LogContext.PushProperty("IP", ip))
            using (LogContext.PushProperty("UserId", userId))
            {
                _logger.LogInformation("{ServiceName}: {Message}: {IP}", ServiceName, message, ip);
            }
        }

        protected void LogResponse(ResponseModel response, string message = "")
        {
            var ip = GetRequestIpAddress();
            var userId = GetRequestUserId();

            using (LogContext.PushProperty("IP", ip))
            using (LogContext.PushProperty("UserId", userId.Value))
            {
                if (response.Success)
                {
                    _logger.LogInformation("{Service}: {Message}: {IP}", ServiceName, message, ip);
                    return;
                }

                using(LogContext.PushProperty("Error", response.Error))
                _logger.LogWarning("{ServiceName}: {Message} - Error: {Error}: {IP}", ServiceName, message, response.Error, ip);
            }
        }
        
        protected string? GetRequestIpAddress() 
            => _httpContextAccessor.HttpContext?.Connection.RemoteIpAddress?.ToString();

        protected long? GetRequestUserId() 
            => _httpContextAccessor.HttpContext?.User.Claims
                .Where(c => c.Type == ClaimTypes.NameIdentifier)
                .Select(c => long.Parse(c.Value))
                .FirstOrDefault();
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

        public async Task<IEnumerable<TModel>> GetAsync(TGetRequest request)
        {
            LogInformation("Get");

            return await _service.GetAsync(request);
        }

        public async Task<int> GetCountAsync(TGetRequest request)
        {
            LogInformation("Get Count");

            return await _service.GetCountAsync(request);
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

        public async Task<TModel> GetByIdAsync(long id)
        {
            LogInformation("Get By Id");

            return await _service.GetByIdAsync(id);
        }
    }
}
