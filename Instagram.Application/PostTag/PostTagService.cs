using Instagram.Application.Abstraction;
using Instagram.Database.Repository;
using Instagram.Domain.Entity;
using Instagram.Models.PostTag;
using Instagram.Models.PostTag.Request;
using Instagram.Models.Response;
using System.Transactions;

namespace Instagram.Application
{
    public class PostTagService : KeylessServiceBase<PostTag, PostTagModel, GetPostTagRequest, IPostTagRepository>, IPostTagService
    {
        private readonly IPostTagFactory _postTagFactory;
        private readonly IResponseFactory _responseFactory;

        public PostTagService(IPostTagRepository repository, IPostTagFactory postTagFactory, IResponseFactory responseFactory) : base(repository)
        {
            _postTagFactory = postTagFactory;
            _responseFactory = responseFactory;
        }

        public async Task<ResponseModel> AddAsync(AddPostTagRequest request)
        {
            try
            {
                var tags = _postTagFactory.CreateMany(request);

                using(var scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
                {
                    foreach(var tag in tags)
                    {
                        await _repository.InsertAsync(tag);
                    }

                    scope.Complete();
                }

                return _responseFactory.CreateSuccess();
            }
            catch (Exception ex)
            {
                return _responseFactory.CreateFailure(ex.Message);
            }
        }

        public async Task<ResponseModel> DeleteAsync(long postId, string tag)
        {
            try
            {
                await _repository.DeleteAsync(postId, tag);

                return _responseFactory.CreateSuccess();
            }
            catch (Exception ex)
            {
                return _responseFactory.CreateFailure(ex.Message);
            }
        }
    }
}
