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

        public Task<ResponseModel> AddAsync(AddPostLikeRequest request)
        {
            throw new NotImplementedException();
        }

        public Task<ResponseModel> DeleteAsync(long postId, long userId)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<PostLikeModel>> GetAsync(GetPostLikeRequest request)
        {
            throw new NotImplementedException();
        }

        public Task<int> GetCountAsync(GetPostLikeRequest request)
        {
            throw new NotImplementedException();
        }
    }
}
