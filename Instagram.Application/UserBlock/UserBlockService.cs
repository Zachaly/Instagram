using Instagram.Application.Abstraction;
using Instagram.Database.Repository;
using Instagram.Domain.Entity;
using Instagram.Models.Response;
using Instagram.Models.UserBlock;
using Instagram.Models.UserBlock.Request;

namespace Instagram.Application
{
    public class UserBlockService : ServiceBase<UserBlock, UserBlockModel, GetUserBlockRequest, AddUserBlockRequest, IUserBlockRepository>,
        IUserBlockService
    {

        public UserBlockService(IUserBlockRepository repository,IUserBlockFactory userBlockFactory,
            IResponseFactory responseFactory) : base(repository, userBlockFactory, responseFactory)
        {

        }

        public async Task<ResponseModel> DeleteByIdAsync(long id)
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
