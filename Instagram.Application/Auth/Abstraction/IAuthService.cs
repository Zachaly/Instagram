using Instagram.Domain.Entity;

namespace Instagram.Application.Abstraction
{
    public interface IAuthService
    {
        Task<string> HashPasswordAsync(string password);
        Task<bool> VerifyPasswordAsync(string password, string hash);
        Task<string> GenerateTokenAsync(User user, IEnumerable<UserClaim> userClaims);
    }
}
