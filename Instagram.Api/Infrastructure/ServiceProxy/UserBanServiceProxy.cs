using Instagram.Application.Abstraction;
using Instagram.Models.Response;
using Instagram.Models.UserBan;
using Instagram.Models.UserBan.Request;

namespace Instagram.Api.Infrastructure.ServiceProxy
{
    public interface IUserBanServiceProxy : IUserBanService
    {
        
    }

    public class UserBanServiceProxy : HttpLoggingProxyBase<IUserBanService>, IUserBanServiceProxy
    {
        private readonly IUserBanService _service;

        public UserBanServiceProxy(ILogger<IUserBanService> logger, IHttpContextAccessor httpContextAccessor,
            IUserBanService userBanService) : base(logger, httpContextAccessor)
        {
            _service = userBanService;
        }

        public async Task<ResponseModel> AddAsync(AddUserBanRequest request)
        {
            LogInformation("Add");

            var response = await _service.AddAsync(request);

            LogResponse(response, "Add");

            return response;
        }

        public async Task<ResponseModel> DeleteAsync(long id)
        {
            LogInformation("Delete");

            var response = await _service.DeleteAsync(id);

            LogResponse(response, "Delete");

            return response;
        }

        public Task<IEnumerable<UserBanModel>> GetAsync(GetUserBanRequest request)
        {
            LogInformation("Get");

            return _service.GetAsync(request);
        }

        public Task<UserBanModel> GetByIdAsync(long id)
        {
            LogInformation("Get By Id");

            return _service.GetByIdAsync(id);
        }

        public Task<int> GetCountAsync(GetUserBanRequest request)
        {
            LogInformation("Get Count");

            return _service.GetCountAsync(request);
        }
    }
}
