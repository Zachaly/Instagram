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

        public async Task<ResponseModel> AddAsync(AddUserClaimRequest request)
        {
            try
            {
                var claim = _userClaimFactory.Create(request);

                await _repository.InsertAsync(claim);

                return _responseFactory.CreateSuccess();
            }
            catch(Exception ex)
            {
                return _responseFactory.CreateFailure(ex.Message);
            }
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
