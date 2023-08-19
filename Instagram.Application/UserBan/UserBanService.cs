using Instagram.Application.Abstraction;
using Instagram.Database.Repository;
using Instagram.Domain.Entity;
using Instagram.Models.Response;
using Instagram.Models.UserBan;
using Instagram.Models.UserBan.Request;

namespace Instagram.Application
{
    public class UserBanService : ServiceBase<UserBan, UserBanModel, GetUserBanRequest, AddUserBanRequest, IUserBanRepository>, IUserBanService
    {

        public UserBanService(IUserBanRepository repository, IUserBanFactory userBanFactory, IResponseFactory responseFactory)
            : base(repository, userBanFactory, responseFactory)
        {
        }

        public async Task<ResponseModel> DeleteAsync(long id)
        {
            try
            {
                await _repository.DeleteByIdAsync(id);

                return _responseFactory.CreateSuccess();
            }
            catch (Exception ex)
            {
                return _responseFactory.CreateFailure(ex.Message);
            }
        }
    }
}
