using Instagram.Application.Abstraction;
using Instagram.Database.Repository;
using Instagram.Models.Response;
using Instagram.Models.User;
using Instagram.Models.User.Request;

namespace Instagram.Application
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly IResponseFactory _responseFactory;

        public UserService(IUserRepository userRepository, IResponseFactory responseFactory)
        {
            _userRepository = userRepository;
            _responseFactory = responseFactory;
        }

        public Task<IEnumerable<UserModel>> GetAsync(GetUserRequest request)
        {
            return _userRepository.GetAsync(request);
        }

        public Task<UserModel> GetByIdAsync(long id)
        {
            return _userRepository.GetByIdAsync(id);
        }

        public Task<ResponseModel> UpdateAsync(UpdateUserRequest request)
        {
            throw new NotImplementedException();
        }
    }
}
