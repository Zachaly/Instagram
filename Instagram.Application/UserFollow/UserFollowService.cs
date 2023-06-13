using Instagram.Application.Abstraction;
using Instagram.Database.Repository;
using Instagram.Domain.Entity;
using Instagram.Models.Response;
using Instagram.Models.UserFollow;
using Instagram.Models.UserFollow.Request;

namespace Instagram.Application
{
    public class UserFollowService : KeylessServiceBase<UserFollow, UserFollowModel, GetUserFollowRequest, IUserFollowRepository>, IUserFollowService
    {
        private readonly IUserFollowFactory _userFollowFactory;
        private readonly IResponseFactory _responseFactory;

        public UserFollowService(IUserFollowRepository userFollowRepository, IUserFollowFactory userFollowFactory, 
            IResponseFactory responseFactory) : base(userFollowRepository)
        {
            _userFollowFactory = userFollowFactory;
            _responseFactory = responseFactory;
        }

        public async Task<ResponseModel> AddAsync(AddUserFollowRequest request)
        {
            try
            {
                var follow = _userFollowFactory.Create(request);

                await _repository.InsertAsync(follow);

                return _responseFactory.CreateSuccess();
            }
            catch (Exception ex)
            {
                return _responseFactory.CreateFailure(ex.Message);
            }
        }

        public async Task<ResponseModel> DeleteAsync(long followerId, long followedId)
        {
            try
            {
                await _repository.DeleteAsync(followerId, followedId);

                return _responseFactory.CreateSuccess();
            }
            catch (Exception ex)
            {
                return _responseFactory.CreateFailure(ex.Message);
            }
        }
    }
}
