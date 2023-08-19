using Instagram.Domain.Entity;
using Instagram.Models.PostComment.Request;

namespace Instagram.Application.Abstraction
{
    public interface IPostCommentFactory : IEntityFactory<PostComment, AddPostCommentRequest>
    {

    }
}
