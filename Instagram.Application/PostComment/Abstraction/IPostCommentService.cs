using Instagram.Models.PostComment;
using Instagram.Models.PostComment.Request;

namespace Instagram.Application.Abstraction
{
    public interface IPostCommentService : IServiceBase<PostCommentModel, GetPostCommentRequest, AddPostCommentRequest>
    {
    }
}
