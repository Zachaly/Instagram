using Instagram.Application.Abstraction;
using Instagram.Database.Repository;
using Instagram.Models.User;
using Instagram.Models.User.Request;

namespace Instagram.Application
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;

        public UserService(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public Task<IEnumerable<UserModel>> GetAsync(GetUserRequest request)
        {
            return _userRepository.GetAsync(request);
        }

        public Task<UserModel> GetByIdAsync(long id)
        {
            return _userRepository.GetByIdAsync(id);
        }
    }
}
