using Instagram.Database.Repository.Abstraction;
using Instagram.Domain.Entity;
using Instagram.Models.UserClaim;
using Instagram.Models.UserClaim.Request;

namespace Instagram.Database.Repository
{
    public interface IUserClaimRepository : IKeylessRepositoryBase<UserClaim, UserClaimModel, GetUserClaimRequest>
    {
        Task DeleteAsync(long userId, string value);
        Task<IEnumerable<UserClaim>> GetEntitiesAsync(GetUserClaimRequest request);
    }
}
