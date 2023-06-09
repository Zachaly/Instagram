using Instagram.Application.Abstraction;
using Instagram.Database.Repository;
using Instagram.Models.Response;
using Instagram.Models.UserFollow;
using Instagram.Models.UserFollow.Request;

namespace Instagram.Application
{
    public class UserFollowService : IUserFollowService
    {
        private readonly IUserFollowRepository _userFollowRepository;
        private readonly IUserFollowFactory _userFollowFactory;
        private readonly IResponseFactory _responseFactory;

        public UserFollowService(IUserFollowRepository userFollowRepository, IUserFollowFactory userFollowFactory, 
            IResponseFactory responseFactory)
        {
            _userFollowRepository = userFollowRepository;
            _userFollowFactory = userFollowFactory;
            _responseFactory = responseFactory;
        }

        public async Task<ResponseModel> AddAsync(AddUserFollowRequest request)
        {
            try
            {
                var follow = _userFollowFactory.Create(request);

                await _userFollowRepository.InsertAsync(follow);

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
                await _userFollowRepository.DeleteAsync(followerId, followedId);

                return _responseFactory.CreateSuccess();
            }
            catch (Exception ex)
            {
                return _responseFactory.CreateFailure(ex.Message);
            }
        }

        public Task<IEnumerable<UserFollowModel>> GetAsync(GetUserFollowRequest request)
        {
            return _userFollowRepository.GetAsync(request);
        }

        public Task<int> GetCountAsync(GetUserFollowRequest request)
        {
            return _userFollowRepository.GetCountAsync(request);
        }
    }
}
