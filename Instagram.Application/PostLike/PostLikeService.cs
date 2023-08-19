using Instagram.Application.Abstraction;
using Instagram.Database.Repository;
using Instagram.Domain.Entity;
using Instagram.Models.PostLike;
using Instagram.Models.PostLike.Request;
using Instagram.Models.Response;

namespace Instagram.Application
{
    public class PostLikeService : KeylessServiceBase<PostLike, PostLikeModel, GetPostLikeRequest, AddPostLikeRequest, IPostLikeRepository>,
        IPostLikeService
    {
        public PostLikeService(IPostLikeFactory postLikeFactory, IPostLikeRepository postLikeRepository,
            IResponseFactory responseFactory) : base(postLikeRepository, postLikeFactory, responseFactory)
        {
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
