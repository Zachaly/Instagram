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
        private readonly Mock<IPostImageRepository> _postImageRepository;
        private readonly AddPostHandler _handler;

        public AddPostCommandTests()
        {
            _postRepository = new Mock<IPostRepository>();
            _postFactory = new Mock<IPostFactory>();
            _responseFactory = new Mock<IResponseFactory>();
            _fileService = new Mock<IFileService>();
            _postImageRepository = new Mock<IPostImageRepository>();
            _handler = new AddPostHandler(_postFactory.Object, _postRepository.Object, _fileService.Object,
                _responseFactory.Object, _postImageRepository.Object);
        }

        [Fact]
        public async Task Handle_Success()
        {
            var posts = new List<Post>();
            var images = new List<PostImage>();
            const string FileName = "file";
            const long PostId = 1;

            _fileService.Setup(x => x.SavePostImagesAsync(It.IsAny<IEnumerable<IFormFile>>())).ReturnsAsync(new string[] { FileName });

            _postRepository.Setup(x => x.InsertAsync(It.IsAny<Post>()))
                .Callback((Post post) => posts.Add(post))
                .ReturnsAsync(PostId);

            _postFactory.Setup(x => x.Create(It.IsAny<AddPostRequest>()))
                .Returns((AddPostRequest request) => new Post { CreatorId = request.CreatorId });

            _postFactory.Setup(x => x.CreateImages(It.IsAny<IEnumerable<string>>(), It.IsAny<long>()))
                .Returns((IEnumerable<string> files, long id) => files.Select(x => new PostImage { File = x, PostId = id }));

            _responseFactory.Setup(x => x.CreateSuccess())
                .Returns(new ResponseModel { Success = true });

            _postImageRepository.Setup(x => x.InsertAsync(It.IsAny<PostImage>()))
                .Callback((PostImage img) => images.Add(img));

            var file = new Mock<IFormFile>();

            var command = new AddPostCommand
            {
                CreatorId = 1,
                Files = new IFormFile[] { file.Object },
            };

            var res = await _handler.Handle(command, default);

            Assert.True(res.Success);
            Assert.Contains(posts, x => x.CreatorId == command.CreatorId);
            Assert.Contains(images, x => x.PostId == PostId && x.File == FileName);
            Assert.Single(images);
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
        public async Task Handle_FilesEmpty_Fail()
        {
            _responseFactory.Setup(x => x.CreateFailure(It.IsAny<string>()))
                .Returns((string error) => new ResponseModel { Success = false, Error = error });

            var command = new AddPostCommand
            {
                CreatorId = 1,
                Files = new IFormFile[] { },
            };

            var res = await _handler.Handle(command, default);

            Assert.False(res.Success);
            Assert.NotEmpty(res.Error);
        }

        [Fact]
        public async Task Handle_ExceptionThrown_Fail()
        {
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
