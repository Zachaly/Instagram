using Instagram.Application.Abstraction;
using Instagram.Models.Response;
using Instagram.Models.UserClaim;
using Instagram.Models.UserClaim.Request;

namespace Instagram.Api.Infrastructure.ServiceProxy
{
    public interface IUserClaimServiceProxy : IUserClaimService { }

    public class UserClaimServiceProxy : HttpLoggingProxyBase<IUserClaimService>, IUserClaimServiceProxy
    {
        private readonly IUserClaimService _service;

        public UserClaimServiceProxy(ILogger<IUserClaimService> logger, IHttpContextAccessor httpContextAccessor,
            IUserClaimService service) : base(logger, httpContextAccessor)
        {
            _service = service;
        }

        public async Task<ResponseModel> AddAsync(AddUserClaimRequest request)
        {
            LogInformation("Add");

            var response = await _service.AddAsync(request);

            LogResponse(response, "Add");

            return response;
        }

        public async Task<ResponseModel> DeleteAsync(long userId, string value)
        {
            LogInformation("Delete");

            var response = await _service.DeleteAsync(userId, value);

            LogResponse(response, "Delete");

            return response;
        }

        public Task<IEnumerable<UserClaimModel>> GetAsync(GetUserClaimRequest request)
        {
            LogInformation("Get");

            return _service.GetAsync(request);
        }

        public Task<int> GetCountAsync(GetUserClaimRequest request)
        {
            LogInformation("Get Count");

            return _service.GetCountAsync(request);
        }
    }
}
