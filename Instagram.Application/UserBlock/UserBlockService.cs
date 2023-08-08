using Instagram.Application.Abstraction;
using Instagram.Database.Repository;
using Instagram.Domain.Entity;
using Instagram.Models.Response;
using Instagram.Models.UserBlock;
using Instagram.Models.UserBlock.Request;

namespace Instagram.Application
{
    public class UserBlockService : ServiceBase<UserBlock, UserBlockModel, GetUserBlockRequest, IUserBlockRepository>, IUserBlockService
    {
        private readonly IResponseFactory _responseFactory;
        private readonly IUserBlockFactory _userBlockFactory;

        public UserBlockService(IUserBlockRepository repository,IUserBlockFactory userBlockFactory,
            IResponseFactory responseFactory) : base(repository)
        {
            _responseFactory = responseFactory;
            _userBlockFactory = userBlockFactory;
        }

        public Task<ResponseModel> AddAsync(AddUserBlockRequest request)
        {
            throw new NotImplementedException();
        }

        public Task<ResponseModel> DeleteByIdAsync(long id)
        {
            throw new NotImplementedException();
        }
    }
}
