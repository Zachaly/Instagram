using Instagram.Application.Abstraction;
using Instagram.Database.Repository;
using Instagram.Domain.Entity;
using Instagram.Models.DirectMessage;
using Instagram.Models.DirectMessage.Request;
using Instagram.Models.Response;

namespace Instagram.Application
{
    public class DirectMessageService : ServiceBase<DirectMessage, DirectMessageModel, GetDirectMessageRequest, AddDirectMessageRequest, IDirectMessageRepository>,
        IDirectMessageService
    {
        public DirectMessageService(IDirectMessageRepository repository, IDirectMessageFactory directMessageFactory,
            IResponseFactory responseFactory) : base(repository, directMessageFactory, responseFactory)
        {
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
