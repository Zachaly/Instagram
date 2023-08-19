using Instagram.Models.Response;
using Serilog.Context;
using System.Security.Claims;

namespace Instagram.Api.Infrastructure.ServiceProxy.Abstraction
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

                using (LogContext.PushProperty("Error", response.Error))
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
}
