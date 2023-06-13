using Instagram.Application.Abstraction;
using Instagram.Database.Repository;
using Instagram.Domain.Entity;
using Instagram.Models.Response;
using Instagram.Models.User;
using Instagram.Models.User.Request;

namespace Instagram.Application
{
    public class UserService : ServiceBase<User, UserModel, GetUserRequest, IUserRepository>, IUserService
    {
        private readonly IResponseFactory _responseFactory;

        public UserService(IUserRepository userRepository, IResponseFactory responseFactory)
            : base(userRepository)
        {
            _responseFactory = responseFactory;
        }

        public async Task<ResponseModel> UpdateAsync(UpdateUserRequest request)
        {
            try
            {
                await _repository.UpdateAsync(request);

                return _responseFactory.CreateSuccess();
            }
            catch(Exception ex)
            {
                return _responseFactory.CreateFailure(ex.Message);
            }
        }
    }
}
