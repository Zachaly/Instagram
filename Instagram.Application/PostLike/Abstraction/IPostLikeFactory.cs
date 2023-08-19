using Instagram.Domain.Entity;
using Instagram.Models.PostLike.Request;

namespace Instagram.Application.Abstraction
{
    public interface IPostLikeFactory : IEntityFactory<PostLike, AddPostLikeRequest>
    {

    }
}
