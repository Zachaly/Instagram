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

        public async Task<ResponseModel> AddAsync(AddPostCommentRequest request)
        {
            try
            {
                var comment = _postCommentFactory.Create(request);

                var id = await _repository.InsertAsync(comment);

                return _responseFactory.CreateSuccess(id);
            }
            catch (Exception ex)
            {
                return _responseFactory.CreateFailure(ex.Message);
            }
        }
    }
}
