using Instagram.Application.Abstraction;
using Instagram.Database.Repository;
using Instagram.Models.PostLike;
using Instagram.Models.PostLike.Request;
using Instagram.Models.Response;

namespace Instagram.Application
{
    public class PostLikeService : IPostLikeService
    {
        private readonly IPostLikeFactory _postLikeFactory;
        private readonly IPostLikeRepository _postLikeRepository;
        private readonly IResponseFactory _responseFactory;

        public PostLikeService(IPostLikeFactory postLikeFactory, IPostLikeRepository postLikeRepository,
            IResponseFactory responseFactory)
        {
            _postLikeFactory = postLikeFactory;
            _postLikeRepository = postLikeRepository;
            _responseFactory = responseFactory;
        }

        public async Task<ResponseModel> AddAsync(AddPostLikeRequest request)
        {
            try
            {
                var like = _postLikeFactory.Create(request);

                await _postLikeRepository.InsertAsync(like);

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
                await _postLikeRepository.DeleteAsync(postId, userId);

                return _responseFactory.CreateSuccess();
            }
            catch(Exception ex)
            {
                return _responseFactory.CreateFailure(ex.Message);
            }
        }

        public Task<IEnumerable<PostLikeModel>> GetAsync(GetPostLikeRequest request)
        {
            return _postLikeRepository.GetAsync(request);
        }

        public Task<int> GetCountAsync(GetPostLikeRequest request)
        {
            return _postLikeRepository.GetCountAsync(request);
        }
    }
}
