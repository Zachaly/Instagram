using Instagram.Application.Abstraction;
using Instagram.Database.Repository;
using Instagram.Domain.Entity;
using Instagram.Models.DirectMessage;
using Instagram.Models.DirectMessage.Request;
using Instagram.Models.Response;

namespace Instagram.Application
{
    public class DirectMessageService : ServiceBase<DirectMessage, DirectMessageModel, GetDirectMessageRequest, IDirectMessageRepository>, IDirectMessageService
    {
        private readonly IDirectMessageFactory _directMessageFactory;
        private readonly IResponseFactory _responseFactory;

        public DirectMessageService(IDirectMessageRepository repository, IDirectMessageFactory directMessageFactory,
            IResponseFactory responseFactory) : base(repository)
        {
            _directMessageFactory = directMessageFactory;
            _responseFactory = responseFactory;
        }

        public async Task<ResponseModel> AddAsync(AddDirectMessageRequest request)
        {
            try
            {
                var message = _directMessageFactory.Create(request);

                var id = await _repository.InsertAsync(message);

                return _responseFactory.CreateSuccess(id);
            }
            catch(Exception ex)
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
            catch(Exception ex)
            {
                return _responseFactory.CreateFailure(ex.Message);
            }
        }

        public async Task<ResponseModel> UpdateAsync(UpdateDirectMessageRequest request)
        {
            try
            {
                await _repository.UpdateAsync(request);

                return _responseFactory.CreateSuccess();
            }
            catch (Exception ex)
            {
                return _responseFactory.CreateFailure(ex.Message);
            }
        }
    }
}
