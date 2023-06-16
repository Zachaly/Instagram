using Instagram.Application;
using Instagram.Application.Abstraction;
using Instagram.Database.Repository;
using Instagram.Domain.Entity;
using Instagram.Models.PostComment.Request;
using Instagram.Models.Response;
using Moq;

namespace Instagram.Tests.Unit.ServiceTests
{
    public class PostCommentServiceTests
    {
        private readonly Mock<IPostCommentRepository> _postCommentRepository;
        private readonly Mock<IPostCommentFactory> _postCommentFactory;
        private readonly Mock<IResponseFactory> _responseFactory;
        private readonly PostCommentService _service;

        public PostCommentServiceTests()
        {
            _postCommentRepository = new Mock<IPostCommentRepository>();
            _postCommentFactory = new Mock<IPostCommentFactory>();
            _responseFactory = new Mock<IResponseFactory>();

            _service = new PostCommentService(_postCommentRepository.Object, _postCommentFactory.Object, _responseFactory.Object);
        }

        [Fact]
        public async Task AddAsync_AddsComment()
        {
            var comments = new List<PostComment>();

            const long NewCommentId = 1;

            _postCommentFactory.Setup(x => x.Create(It.IsAny<AddPostCommentRequest>()))
                .Returns((AddPostCommentRequest req) => new PostComment { Content = req.Content });

            _postCommentRepository.Setup(x => x.InsertAsync(It.IsAny<PostComment>()))
                .Callback((PostComment comment) => comments.Add(comment))
                .ReturnsAsync(NewCommentId);

            _responseFactory.Setup(x => x.CreateSuccess(It.IsAny<long>()))
                .Returns((long id) => new ResponseModel { Success = true, NewEntityId = id });

            var request = new AddPostCommentRequest { Content = "content" };

            var res = await _service.AddAsync(request);

            Assert.True(res.Success);
            Assert.Equal(NewCommentId, res.NewEntityId);
            Assert.Contains(comments, x => x.Content == request.Content);
        }

        [Fact]
        public async Task AddAsync_ExceptionThrown_Failure()
        {
            const string Error = "err";

            _postCommentFactory.Setup(x => x.Create(It.IsAny<AddPostCommentRequest>()))
                .Returns((AddPostCommentRequest req) => new PostComment { Content = req.Content });

            _postCommentRepository.Setup(x => x.InsertAsync(It.IsAny<PostComment>()))
                .Callback(() => throw new Exception(Error));

            _responseFactory.Setup(x => x.CreateFailure(It.IsAny<string>()))
                .Returns((string error) => new ResponseModel { Success = false, Error = error });

            var request = new AddPostCommentRequest { Content = "content" };

            var res = await _service.AddAsync(request);

            Assert.False(res.Success);
            Assert.Equal(Error, res.Error);
        }
    }
}
