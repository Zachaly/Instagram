using Instagram.Domain.Entity;
using Instagram.Models.PostComment.Request;

namespace Instagram.Application.Abstraction
{
    public interface IPostCommentFactory
    {
        PostComment Create(AddPostCommentRequest request);
    }
}
