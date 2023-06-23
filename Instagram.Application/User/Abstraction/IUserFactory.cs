using Instagram.Domain.Entity;
using Instagram.Models.User;
using Instagram.Models.User.Request;

namespace Instagram.Application.Abstraction
{
    public interface IUserFactory
    {
        User Create(RegisterRequest request, string passwordHash);
        LoginResponse CreateLoginResponse(long userId, string token, string email, IEnumerable<UserClaim> claims);
    }
}
