using Instagram.Application.Abstraction;
using Instagram.Database.Repository;
using Instagram.Domain.Entity;
using Instagram.Models.PostLike;
using Instagram.Models.PostLike.Request;
using Instagram.Models.Response;

namespace Instagram.Application
{
    public class PostLikeService : KeylessServiceBase<PostLike, PostLikeModel, GetPostLikeRequest, IPostLikeRepository>, IPostLikeService
    {
        private readonly IPostLikeFactory _postLikeFactory;
        private readonly IResponseFactory _responseFactory;

        public PostLikeService(IPostLikeFactory postLikeFactory, IPostLikeRepository postLikeRepository,
            IResponseFactory responseFactory) : base(postLikeRepository)
        {
            _postLikeFactory = postLikeFactory;
            _responseFactory = responseFactory;
        }

        public async Task<ResponseModel> AddAsync(AddPostLikeRequest request)
        {
            try
            {
                var like = _postLikeFactory.Create(request);

                await _repository.InsertAsync(like);

                return _responseFactory.CreateSuccess();
            }
            catch(Exception ex)
            {
                return _responseFactory.CreateFailure(ex.Message);
            }
        }

        public async Task<ResponseModel> DeleteAsync(long postId, long userId)
        {
            try
            {
                await _repository.DeleteAsync(postId, userId);

                return _responseFactory.CreateSuccess();
            }
            catch(Exception ex)
            {
                return _responseFactory.CreateFailure(ex.Message);
            }
        }
    }
}
