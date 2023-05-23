using Instagram.Application.Abstraction;
using Instagram.Database.Repository;
using Instagram.Models.User;
using Instagram.Models.User.Request;

namespace Instagram.Application
{
    public class UserService : IUserService
    {
        public UserService(IUserFactory userFactory, IUserRepository userRepository, IResponseFactory responseFactory)
        {

        }

        public Task<IEnumerable<UserModel>> GetAsync(GetUserRequest request)
        {
            throw new NotImplementedException();
        }

        public Task<UserModel> GetByIdAsync(long id)
        {
            throw new NotImplementedException();
        }
    }
}
