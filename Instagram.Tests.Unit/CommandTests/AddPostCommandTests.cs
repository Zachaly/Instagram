using Instagram.Application.Abstraction;
using Instagram.Application.Command;
using Instagram.Database.Repository;
using Instagram.Domain.Entity;
using Instagram.Models.Post.Request;
using Instagram.Models.Response;
using Microsoft.AspNetCore.Http;
using Moq;

namespace Instagram.Tests.Unit.CommandTests
{
    public class AddPostCommandTests
    {
        private readonly Mock<IPostRepository> _postRepository;
        private readonly Mock<IPostFactory> _postFactory;
        private readonly Mock<IResponseFactory> _responseFactory;
        private readonly Mock<IFileService> _fileService;
        private readonly AddPostHandler _handler;

        public AddPostCommandTests()
        {
            _postRepository = new Mock<IPostRepository>();
            _postFactory = new Mock<IPostFactory>();
            _responseFactory = new Mock<IResponseFactory>();
            _fileService = new Mock<IFileService>();
            _handler = new AddPostHandler(_postFactory.Object, _postRepository.Object, _fileService.Object, _responseFactory.Object);
        }

        [Fact]
        public async Task Handle_Success()
        {
            var posts = new List<Post>();
            const string FileName = "file";

            _fileService.Setup(x => x.SavePostImagesAsync(It.IsAny<IEnumerable<IFormFile>>())).ReturnsAsync(new string[] { FileName });

            _postRepository.Setup(x => x.InsertAsync(It.IsAny<Post>()))
                .Callback((Post post) => posts.Add(post));

            _postFactory.Setup(x => x.Create(It.IsAny<AddPostRequest>()))
                .Returns((AddPostRequest request) => new Post { CreatorId = request.CreatorId });

            _responseFactory.Setup(x => x.CreateSuccess())
                .Returns(new ResponseModel { Success = true });

            var file = new Mock<IFormFile>();

            var command = new AddPostCommand
            {
                CreatorId = 1,
                Files = new IFormFile[] { file.Object },
            };

            var res = await _handler.Handle(command, default);

            Assert.True(res.Success);
            Assert.Contains(posts, x => x.CreatorId == command.CreatorId);
        }

        [Fact]
        public async Task Handle_FileNull_Fail()
        {
            _responseFactory.Setup(x => x.CreateFailure(It.IsAny<string>()))
                .Returns((string error) => new ResponseModel { Success = false, Error = error });

            var command = new AddPostCommand
            {
                CreatorId = 1,
                Files = null,
            };

            var res = await _handler.Handle(command, default);

            Assert.False(res.Success);
            Assert.NotEmpty(res.Error);
        }

        [Fact]
        public async Task Handle_ExceptionThrown_Fail()
        {
            var posts = new List<Post>();
            const string Error = "error";

            _fileService.Setup(x => x.SavePostImagesAsync(It.IsAny<IEnumerable<IFormFile>>()))
                .Callback(() => throw new Exception(Error));

            _responseFactory.Setup(x => x.CreateFailure(It.IsAny<string>()))
                .Returns((string error) => new ResponseModel { Success = false, Error = error });

            var file = new Mock<IFormFile>();

            var command = new AddPostCommand
            {
                CreatorId = 1,
                Files = new IFormFile[] { file.Object },
            };

            var res = await _handler.Handle(command, default);

            Assert.False(res.Success);
            Assert.Equal(Error, res.Error);
        }
    }
}
