using Instagram.Application.Abstraction;
using Instagram.Database.Repository;
using Instagram.Domain.Entity;
using Instagram.Models.Response;
using Instagram.Models.UserClaim;
using Instagram.Models.UserClaim.Request;

namespace Instagram.Application
{
    public class UserClaimService : KeylessServiceBase<UserClaim, UserClaimModel, GetUserClaimRequest, AddUserClaimRequest, IUserClaimRepository>,
        IUserClaimService
    {
        public UserClaimService(IUserClaimRepository repository, IUserClaimFactory userClaimFactory, IResponseFactory responseFactory)
            : base(repository, userClaimFactory, responseFactory)
        {
        }

        public async Task<ResponseModel> DeleteAsync(long userId, string value)
        {
            try
            {
                await _repository.DeleteAsync(userId, value);

                return _responseFactory.CreateSuccess();
            }
            catch(Exception ex)
            {
                return _responseFactory.CreateFailure(ex.Message);
            }
        }
    }
}
