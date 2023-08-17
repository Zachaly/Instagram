using Instagram.Application;
using Instagram.Application.Abstraction;
using Instagram.Database.Repository;
using Instagram.Domain.Entity;
using Instagram.Models.PostComment.Request;
using Instagram.Models.Response;
using NSubstitute;
using NSubstitute.ExceptionExtensions;

namespace Instagram.Tests.Unit.ServiceTests
{
    public class PostCommentServiceTests
    {
        private readonly IPostCommentRepository _postCommentRepository;
        private readonly IPostCommentFactory _postCommentFactory;
        private readonly IResponseFactory _responseFactory;
        private readonly PostCommentService _service;

        public PostCommentServiceTests()
        {
            _postCommentRepository = Substitute.For<IPostCommentRepository>();
            _postCommentFactory = Substitute.For<IPostCommentFactory>();
            _responseFactory = ResponseFactoryMock.Create();

            _service = new PostCommentService(_postCommentRepository, _postCommentFactory, _responseFactory);
        }

        [Fact]
        public async Task AddAsync_AddsComment()
        {
            var comments = new List<PostComment>();

            const long NewCommentId = 1;

            _postCommentFactory.Create(Arg.Any<AddPostCommentRequest>())
                .Returns(info => new PostComment { Content = info.Arg<AddPostCommentRequest>().Content });

            _postCommentRepository.InsertAsync(Arg.Any<PostComment>())
                .Returns(NewCommentId)
                .AndDoes(info => comments.Add(info.Arg<PostComment>()));

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

            _postCommentFactory.Create(Arg.Any<AddPostCommentRequest>()).Throws(new Exception(Error));

            var request = new AddPostCommentRequest { Content = "content" };

            var res = await _service.AddAsync(request);

            Assert.False(res.Success);
            Assert.Equal(Error, res.Error);
        }
    }
}
