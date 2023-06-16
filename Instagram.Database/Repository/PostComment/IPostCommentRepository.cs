using Instagram.Database.Repository.Abstraction;
using Instagram.Domain.Entity;
using Instagram.Models.PostComment;
using Instagram.Models.PostComment.Request;

namespace Instagram.Database.Repository
{
    public interface IPostCommentRepository : IRepositoryBase<PostComment, PostCommentModel, GetPostCommentRequest>
    {
    }
}
