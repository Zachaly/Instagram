using Instagram.Application.Auth.Abstraction;
using System.Security.Claims;

namespace Instagram.Api.Infrastructure.Service
{
    public class UserDataService : IUserDataService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public UserDataService(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public Task<long?> GetCurrentUserId()
        {
            var idClaim = _httpContextAccessor.HttpContext?.User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier);

            long? id = idClaim is null ? null : long.Parse(idClaim.Value);

            return Task.FromResult(id);
        }
    }
}
