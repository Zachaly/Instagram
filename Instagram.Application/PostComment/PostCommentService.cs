using Instagram.Application.Abstraction;
using Instagram.Database.Repository;
using Instagram.Domain.Entity;
using Instagram.Models.PostComment;
using Instagram.Models.PostComment.Request;

namespace Instagram.Application
{
    public class PostCommentService : ServiceBase<PostComment, PostCommentModel, GetPostCommentRequest, AddPostCommentRequest, IPostCommentRepository>,
        IPostCommentService
    {
        public PostCommentService(IPostCommentRepository repository, IPostCommentFactory postCommentFactory, IResponseFactory responseFactory) 
            : base(repository, postCommentFactory, responseFactory)
        {
        }
    }
}
