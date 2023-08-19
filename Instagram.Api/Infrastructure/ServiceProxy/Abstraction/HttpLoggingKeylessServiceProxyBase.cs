using FluentValidation;
using Instagram.Application;
using Instagram.Application.Abstraction;
using Instagram.Models;
using Instagram.Models.Response;

namespace Instagram.Api.Infrastructure.ServiceProxy.Abstraction
{
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

    public abstract class HttpLoggingKeylessServiceProxyBase<TModel, TGetRequest, TAddRequest, TService>
        : HttpLoggingKeylessServiceProxyBase<TModel, TGetRequest, TService>, IKeylessServiceBase<TModel, TGetRequest, TAddRequest>
        where TService : IKeylessServiceBase<TModel, TGetRequest, TAddRequest>
        where TModel : IModel
        where TGetRequest : PagedRequest
    {
        protected readonly IResponseFactory _responseFactory;
        private readonly IValidator<TAddRequest> _addValidator;

        protected HttpLoggingKeylessServiceProxyBase(ILogger<TService> logger,
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
