using MediatR;

namespace Instagram.Api.Infrastructure.ServiceProxy
{
    public class LoggingPipeline<TRequest, TResponse> : HttpLoggingProxyBase<TRequest>, IPipelineBehavior<TRequest, TResponse>
    {
        public LoggingPipeline(ILogger<TRequest> logger, IHttpContextAccessor httpContextAccessor) : base(logger, httpContextAccessor)
        {
        }

        public Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
        {
            LogInformation("Handle");

            return next();
        }
    }
}
