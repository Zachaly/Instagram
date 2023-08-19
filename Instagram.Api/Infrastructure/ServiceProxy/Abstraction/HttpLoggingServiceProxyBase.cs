using FluentValidation;
using Instagram.Application;
using Instagram.Application.Abstraction;
using Instagram.Models;
using Instagram.Models.Response;

namespace Instagram.Api.Infrastructure.ServiceProxy.Abstraction
{
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

    public abstract class HttpLoggingServiceProxyBase<TModel, TGetRequest, TAddRequest, TService>
        : HttpLoggingServiceProxyBase<TModel, TGetRequest, TService>, IServiceBase<TModel, TGetRequest, TAddRequest>
        where TService : IServiceBase<TModel, TGetRequest, TAddRequest>
        where TModel : IModel
        where TGetRequest : PagedRequest
    {
        protected readonly IResponseFactory _responseFactory;
        private readonly IValidator<TAddRequest> _addValidator;

        protected HttpLoggingServiceProxyBase(ILogger<TService> logger,
            IHttpContextAccessor httpContextAccessor,
            TService service,
            IResponseFactory responseFactory,
            IValidator<TAddRequest> addValidator) : base(logger, httpContextAccessor, service)
        {
            _responseFactory = responseFactory;
            _addValidator = addValidator;
        }

        public virtual async Task<ResponseModel> AddAsync(TAddRequest request)
        {
            LogInformation("Add");

            var validation = _addValidator.Validate(request);

            var response = validation.IsValid ? await _service.AddAsync(request) : _responseFactory.CreateValidationError(validation);

            LogResponse(response, "Add");

            return response;
        }
    }
}
