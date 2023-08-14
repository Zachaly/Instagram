using MediatR;

namespace Instagram.Api.Infrastructure.ServiceProxy
{
    public class LoggingPipeline<TRequest, TResponse> : HttpLoggingProxyBase<TRequest>, IPipelineBehavior<TRequest, TResponse>
    {
        public LoggingPipeline(ILogger<TRequest> logger, IHttpContextAccessor httpContextAccessor) : base(logger, httpContextAccessor)
        {
            ServiceName = typeof(TRequest).Name;
        }

        public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
        {
            LogInformation("Handle");

            return await next();
        }
    }
}
