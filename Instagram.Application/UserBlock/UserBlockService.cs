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

        public async Task<ResponseModel> AddAsync(AddUserBlockRequest request)
        {
            try
            {
                var block = _userBlockFactory.Create(request);
                var id = await _repository.InsertAsync(block);

                return _responseFactory.CreateSuccess(id);
            }
            catch (Exception ex)
            {
                return _responseFactory.CreateFailure(ex.Message);
            }
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
