using Instagram.Models.Response;
using Instagram.Models.UserClaim;
using Instagram.Models.UserClaim.Request;

namespace Instagram.Application.Abstraction
{
    public interface IUserClaimService : IKeylessServiceBase<UserClaimModel, GetUserClaimRequest>
    {
        Task<ResponseModel> DeleteAsync(long userId, string value);
        Task<ResponseModel> AddAsync(AddUserClaimRequest request);
    }
}
