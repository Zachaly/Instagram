using Instagram.Application.Abstraction;
using Instagram.Domain.Entity;
using Instagram.Models.PostLike.Request;

namespace Instagram.Application
{
    public class PostLikeFactory : IPostLikeFactory
    {
        public PostLike Create(AddPostLikeRequest request)
            => new PostLike
            {
                PostId = request.PostId,
                UserId = request.UserId,
            };
    }
}
