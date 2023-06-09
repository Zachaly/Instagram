using Instagram.Application.Abstraction;
using Instagram.Database.Repository;
using Instagram.Models.Response;
using Instagram.Models.UserFollow;
using Instagram.Models.UserFollow.Request;

namespace Instagram.Application
{
    public class UserFollowService : IUserFollowService
    {
        public UserFollowService(IUserFollowRepository userFollowRepository, IUserFollowFactory userFollowFactory, 
            IResponseFactory responseFactory)
        {

        }

        public Task<ResponseModel> AddAsync(AddUserFollowRequest request)
        {
            throw new NotImplementedException();
        }

        public Task<ResponseModel> DeleteAsync(long followerId, long followedId)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<UserFollowModel>> GetAsync(GetUserFollowRequest request)
        {
            throw new NotImplementedException();
        }
    }
}
