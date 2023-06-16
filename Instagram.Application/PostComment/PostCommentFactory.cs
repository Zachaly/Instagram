using Instagram.Application.Abstraction;
using Instagram.Domain.Entity;
using Instagram.Models.PostComment.Request;

namespace Instagram.Application
{
    public class PostCommentFactory : IPostCommentFactory
    {
        public PostComment Create(AddPostCommentRequest request)
        => new PostComment 
        {
            PostId = request.PostId,
            Content = request.Content,
            UserId = request.UserId,
            Created = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds(),
        };
    }
}
