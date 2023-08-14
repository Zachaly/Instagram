using FluentValidation;
using Instagram.Application;
using Instagram.Application.Abstraction;
using Instagram.Models.Response;
using MediatR;

namespace Instagram.Api.Infrastructure.ServiceProxy
{
    public class ValidationPipeline<TRequest, TResponse> : HttpLoggingProxyBase<TRequest>, IPipelineBehavior<TRequest, ResponseModel>
        where TRequest : IValidatedRequest
    {
        private readonly IResponseFactory _responseFactory;
        private readonly IValidator<TRequest> _validator;

        public ValidationPipeline(ILogger<TRequest> logger, IHttpContextAccessor httpContextAccessor,
            IResponseFactory responseFactory, IValidator<TRequest> validator) : base(logger, httpContextAccessor)
        {
            _responseFactory = responseFactory;
            _validator = validator;
            ServiceName = typeof(TRequest).Name;
        }

        public Task<ResponseModel> Handle(TRequest request, RequestHandlerDelegate<ResponseModel> next, CancellationToken cancellationToken)
        {
            var validation = _validator.Validate(request);

            if (!validation.IsValid)
            {
                var response = _responseFactory.CreateValidationError(validation);

                LogResponse(response, "Validation failure");

                return Task.FromResult(response);
            }

            return next();
        }
    }
}
