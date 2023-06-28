using Instagram.Application.Abstraction;
using Instagram.Database.Repository;
using Instagram.Domain.Entity;
using Instagram.Models.Response;
using Instagram.Models.UserBan;
using Instagram.Models.UserBan.Request;

namespace Instagram.Application
{
    public class UserBanService : ServiceBase<UserBan, UserBanModel, GetUserBanRequest, IUserBanRepository>, IUserBanService
    {
        private readonly IResponseFactory _responseFactory;
        private readonly IUserBanFactory _userBanFactory;

        public UserBanService(IUserBanRepository repository, IUserBanFactory userBanFactory, IResponseFactory responseFactory) : base(repository)
        {
            _responseFactory = responseFactory;
            _userBanFactory = userBanFactory;
        }

        public Task<ResponseModel> AddAsync(AddUserBanRequest request)
        {
            throw new NotImplementedException();
        }
    }
}
