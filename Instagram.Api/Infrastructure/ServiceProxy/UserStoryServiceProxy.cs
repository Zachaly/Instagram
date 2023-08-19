using Instagram.Api.Infrastructure.ServiceProxy.Abstraction;
using Instagram.Application.Abstraction;
using Instagram.Models.UserStory;
using Instagram.Models.UserStory.Request;

namespace Instagram.Api.Infrastructure.ServiceProxy
{
    public interface IUserStoryServiceProxy : IUserStoryService { }

    public class UserStoryServiceProxy : HttpLoggingProxyBase<IUserStoryService>, IUserStoryServiceProxy
    {
        private readonly IUserStoryService _service;

        public UserStoryServiceProxy(ILogger<IUserStoryService> logger, IHttpContextAccessor httpContextAccessor,
            IUserStoryService service) : base(logger, httpContextAccessor)
        {
            _service = service;
            ServiceName = "UserStory";
        }

        public Task<IEnumerable<UserStoryModel>> GetAsync(GetUserStoryRequest request)
        {
            LogInformation("Get");

            return _service.GetAsync(request);
        }
    }
}
