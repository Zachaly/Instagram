using Instagram.Application.Abstraction;
using Instagram.Database.Repository;
using Instagram.Domain.Entity;
using Instagram.Models.PostTag;
using Instagram.Models.PostTag.Request;
using Instagram.Models.Response;

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

        public Task<ResponseModel> AddAsync(AddPostTagRequest request)
        {
            throw new NotImplementedException();
        }

        public Task<ResponseModel> DeleteAsync(long postId, string tag)
        {
            throw new NotImplementedException();
        }
    }
}
