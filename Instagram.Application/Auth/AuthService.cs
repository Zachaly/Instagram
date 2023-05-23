using Instagram.Application.Abstraction;
using Instagram.Domain.Entity;
using Microsoft.Extensions.Configuration;

namespace Instagram.Application
{
    public class AuthService : IAuthService
    {
        public AuthService(IConfiguration configuration)
        {

        }

        public Task<string> GenerateTokenAsync(User user)
        {
            throw new NotImplementedException();
        }

        public Task<string> HashPasswordAsync(string password)
        {
            throw new NotImplementedException();
        }

        public Task<bool> VerifyPasswordAsync(string password, string hash)
        {
            throw new NotImplementedException();
        }
    }
}
