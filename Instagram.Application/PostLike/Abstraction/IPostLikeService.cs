using Instagram.Models.PostLike;
using Instagram.Models.PostLike.Request;
using Instagram.Models.Response;

namespace Instagram.Application.Abstraction
{
    public interface IPostLikeService
    {
        Task<IEnumerable<PostLikeModel>> GetAsync(GetPostLikeRequest request);
        Task<ResponseModel> AddAsync(AddPostLikeRequest request);
        Task<int> GetCountAsync(GetPostLikeRequest request);
        Task<ResponseModel> DeleteAsync(long postId, long userId);
    }
}
