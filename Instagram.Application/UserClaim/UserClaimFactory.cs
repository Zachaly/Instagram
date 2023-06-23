using Instagram.Application.Abstraction;
using Instagram.Domain.Entity;
using Instagram.Models.UserClaim.Request;

namespace Instagram.Application
{
    public class UserClaimFactory : IUserClaimFactory
    {
        public UserClaim Create(AddUserClaimRequest request)
            => new UserClaim
            {
                UserId = request.UserId,
                Value = request.Value,
            };
    }
}
