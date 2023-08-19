using Instagram.Api.Infrastructure.ServiceProxy.Abstraction;
using Instagram.Application.Command;
using Instagram.Models.Response;
using Instagram.Models.User;
using MediatR;

namespace Instagram.Api.Infrastructure.ServiceProxy
{
    public class LoginCommandPipeline<TRequest, TResponse> : HttpLoggingProxyBase<LoginCommand>, IPipelineBehavior<TRequest, TResponse>
        where TRequest : LoginCommand
        where TResponse : DataResponseModel<LoginResponse>
    {
        public LoginCommandPipeline(ILogger<LoginCommand> logger, IHttpContextAccessor httpContextAccessor) : base(logger, httpContextAccessor)
        {
            ServiceName = "Login";
        }

        public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
        {
            var response = await next();

            LogResponse(response, "Login Attempt");

            return response;
        }
    }
}
