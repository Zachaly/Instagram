using Instagram.Database.Repository.Abstraction;
using Instagram.Domain.Entity;
using Instagram.Models.PostLike;
using Instagram.Models.PostLike.Request;

namespace Instagram.Database.Repository
{
    public interface IPostLikeRepository : IKeylessRepositoryBase<PostLike, PostLikeModel, GetPostLikeRequest>
    {
        Task DeleteAsync(long postId, long userId);
    }
}
