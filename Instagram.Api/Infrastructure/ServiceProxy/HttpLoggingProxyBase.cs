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
}
