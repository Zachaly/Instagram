using Instagram.Application.Abstraction;
using Instagram.Database.Repository;
using Instagram.Domain.Entity;
using Instagram.Models.DirectMessage;
using Instagram.Models.DirectMessage.Request;
using Instagram.Models.Response;

namespace Instagram.Application
{
    internal class DirectMessageService : ServiceBase<DirectMessage, DirectMessageModel, GetDirectMessageRequest, IDirectMessageRepository>, IDirectMessageService
    {
        private readonly IDirectMessageFactory _directMessageFactory;
        private readonly IResponseFactory _responseFactory;

        public DirectMessageService(IDirectMessageRepository repository, IDirectMessageFactory directMessageFactory,
            IResponseFactory responseFactory) : base(repository)
        {
            _directMessageFactory = directMessageFactory;
            _responseFactory = responseFactory;
        }

        public Task<ResponseModel> AddAsync(AddDirectMessageRequest request)
        {
            throw new NotImplementedException();
        }

        public Task<ResponseModel> DeleteByIdAsync(long id)
        {
            throw new NotImplementedException();
        }

        public Task<ResponseModel> UpdateAsync(UpdateDirectMessageRequest request)
        {
            throw new NotImplementedException();
        }
    }
}
