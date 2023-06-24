using Instagram.Application.Abstraction;
using Instagram.Domain.Entity;
using Instagram.Models.User;
using Instagram.Models.User.Request;

namespace Instagram.Application
{
    public class UserFactory : IUserFactory
    {
        public User Create(RegisterRequest request, string passwordHash)
            => new User
            {
                Email = request.Email,
                Bio = "",
                Gender = request.Gender,
                Name = request.Name,
                Nickname = request.Nickname,
                PasswordHash = passwordHash
            };

        public LoginResponse CreateLoginResponse(long userId, string token, string email, IEnumerable<UserClaim> claims)
            => new LoginResponse
            {
                UserId = userId,
                AuthToken = token,
                Email = email,
                Claims = claims.Select(c => c.Value)
            };
    }
}
