using Instagram.Application.Abstraction;
using Instagram.Database.Repository;
using Instagram.Domain.Entity;
using Instagram.Models.PostComment;
using Instagram.Models.PostComment.Request;
using Instagram.Models.Response;

namespace Instagram.Application
{
    public class PostCommentService : ServiceBase<PostComment, PostCommentModel, GetPostCommentRequest, IPostCommentRepository>,
        IPostCommentService
    {
        private readonly IPostCommentFactory _postCommentFactory;
        private readonly IResponseFactory _responseFactory;

        public PostCommentService(IPostCommentRepository repository, IPostCommentFactory postCommentFactory, IResponseFactory responseFactory) : base(repository)
        {
            _postCommentFactory = postCommentFactory;
            _responseFactory = responseFactory;
        }

        public Task<ResponseModel> AddAsync(AddPostCommentRequest postComment)
        {
            throw new NotImplementedException();
        }
    }
}
