using Instagram.Models.PostComment;
using Instagram.Models.PostComment.Request;
using Instagram.Models.Response;

namespace Instagram.Application.Abstraction
{
    public interface IPostCommentService : IServiceBase<PostCommentModel, GetPostCommentRequest>
    {
        Task<ResponseModel> AddAsync(AddPostCommentRequest postComment);
    }
}
