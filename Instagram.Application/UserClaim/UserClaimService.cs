using Instagram.Application.Abstraction;
using Instagram.Database.Repository;
using Instagram.Domain.Entity;
using Instagram.Models.Response;
using Instagram.Models.UserClaim;
using Instagram.Models.UserClaim.Request;

namespace Instagram.Application
{
    public class UserClaimService : KeylessServiceBase<UserClaim, UserClaimModel, GetUserClaimRequest, IUserClaimRepository>, IUserClaimService
    {
        private readonly IUserClaimFactory _userClaimFactory;
        private readonly IResponseFactory _responseFactory;

        public UserClaimService(IUserClaimRepository repository, IUserClaimFactory userClaimFactory, IResponseFactory responseFactory) : base(repository)
        {
            _userClaimFactory = userClaimFactory;
            _responseFactory = responseFactory;
        }

        public Task<ResponseModel> AddAsync(AddUserClaimRequest request)
        {
            throw new NotImplementedException();
        }

        public Task<ResponseModel> DeleteAsync(long userId, string value)
        {
            throw new NotImplementedException();
        }
    }
}
