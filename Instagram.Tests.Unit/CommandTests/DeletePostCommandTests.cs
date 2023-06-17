using Instagram.Application.Abstraction;
using Instagram.Application.Command;
using Instagram.Database.Repository;
using Instagram.Domain.Entity;
using Instagram.Models.PostImage;
using Instagram.Models.PostImage.Request;
using Instagram.Models.Response;
using Moq;

namespace Instagram.Tests.Unit.CommandTests
{
    public class DeletePostCommandTests
    {
        private readonly Mock<IPostRepository> _postRepository;
        private readonly Mock<IResponseFactory> _responseFactory;
        private readonly Mock<IFileService> _fileService;
        private readonly Mock<IPostImageRepository> _postImageRepository;
        private readonly Mock<IPostTagRepository> _postTagRepository;
        private readonly DeletePostHandler _handler;

        public DeletePostCommandTests()
        {
            _postRepository = new Mock<IPostRepository>();
            _responseFactory = new Mock<IResponseFactory>();
            _fileService = new Mock<IFileService>();
            _postImageRepository = new Mock<IPostImageRepository>();
            _postTagRepository = new Mock<IPostTagRepository>();
            _handler = new DeletePostHandler(_postRepository.Object, _fileService.Object, _responseFactory.Object,
                _postImageRepository.Object, _postTagRepository.Object);
        }

        [Fact]
        public async Task Handle_Success()
        {
            const int IdToDelete = 2;
            var posts = new List<Post> 
            { 
                new Post { Id = 1 },
                new Post { Id = IdToDelete },
                new Post { Id = 3 },
                new Post { Id = 4 },
            };

            var images = new List<PostImage>
            {
                new PostImage { PostId = IdToDelete, File = "" },
                new PostImage { PostId = IdToDelete, File = "" },
                new PostImage { PostId = 4, File = "" },
                new PostImage { PostId = 5, File = "" },
            };

            var tags = new List<PostTag>
            {
                new PostTag { PostId = 3 },
                new PostTag { PostId = IdToDelete },
                new PostTag { PostId = 1 },
                new PostTag { PostId = IdToDelete },
                new PostTag { PostId = 4 },
                new PostTag { PostId = 5 },
            };

            _postRepository.Setup(x => x.DeleteByIdAsync(It.IsAny<long>()))
                .Callback((long id) => posts.Remove(posts.First(x => x.Id == id)));

            _postRepository.Setup(x => x.GetEntityByIdAsync(It.IsAny<long>()))
                .ReturnsAsync((long id) => posts.First(x => x.Id == id));

            _postImageRepository.Setup(x => x.DeleteByPostIdAsync(It.IsAny<long>()))
                .Callback((long postId) => images.RemoveAll(img => img.PostId == postId));

            _postImageRepository.Setup(x => x.GetAsync(It.IsAny<GetPostImageRequest>()))
                .ReturnsAsync((GetPostImageRequest request) 
                    => images
                        .Where(img => img.PostId == request.PostId)
                        .Select(x => new PostImageModel { File = x.File }));

            _fileService.Setup(x => x.RemovePostImageAsync(It.IsAny<string>()));

            _responseFactory.Setup(x => x.CreateSuccess())
                .Returns(new ResponseModel { Success = true });

            var command = new DeletePostCommand { Id = IdToDelete };

            var res = await _handler.Handle(command, default);

            Assert.True(res.Success);
            Assert.DoesNotContain(posts, x => x.Id == command.Id);
            Assert.DoesNotContain(images, x => x.PostId == command.Id);
            Assert.DoesNotContain(tags, x => x.PostId == command.Id);
        }

        [Fact]
        public async Task Handle_PostNotFound_Fail()
        {
            _postRepository.Setup(x => x.GetEntityByIdAsync(It.IsAny<long>()))
                .ReturnsAsync((long id) => null);

            _responseFactory.Setup(x => x.CreateFailure(It.IsAny<string>()))
                .Returns((string error) => new ResponseModel { Success = false, Error = error });

            var command = new DeletePostCommand { Id = 2137 };

            var res = await _handler.Handle(command, default);

            Assert.False(res.Success);
            Assert.NotEmpty(res.Error);
        }

        [Fact]
        public async Task Handle_ExceptionThrown_Fail()
        {
            const int IdToDelete = 2;
            var posts = new List<Post>
            {
                new Post { Id = 1, },
                new Post { Id = IdToDelete, },
                new Post { Id = 3 },
                new Post { Id = 4 },
            };
            const string Error = "Err";

            _postRepository.Setup(x => x.DeleteByIdAsync(It.IsAny<long>()))
                .Callback((long id) => throw new Exception(Error));

            _postImageRepository.Setup(x => x.GetAsync(It.IsAny<GetPostImageRequest>()));

            _postRepository.Setup(x => x.GetEntityByIdAsync(It.IsAny<long>()))
                .ReturnsAsync((long id) => posts.First(x => x.Id == id));

            _responseFactory.Setup(x => x.CreateFailure(It.IsAny<string>()))
                .Returns((string error) => new ResponseModel { Success = false, Error = error });

            var command = new DeletePostCommand { Id = IdToDelete };

            var res = await _handler.Handle(command, default);

            Assert.False(res.Success);
            Assert.Equal(Error, res.Error);
        }
    }
}
