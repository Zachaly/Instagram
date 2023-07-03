using Instagram.Application.Auth.Abstraction;
using System.Security.Claims;

namespace Instagram.Api.Infrastructure
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

            var claims = _httpContextAccessor.HttpContext.User.Claims.ToList();

            long? id = idClaim is null ? null : long.Parse(idClaim.Value);

            return Task.FromResult(id);
        }
    }
}
