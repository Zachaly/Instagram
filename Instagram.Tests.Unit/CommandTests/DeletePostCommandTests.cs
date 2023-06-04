using Instagram.Application.Abstraction;
using Instagram.Application.Command;
using Instagram.Database.Repository;
using Instagram.Domain.Entity;
using Instagram.Models.Response;
using Moq;

namespace Instagram.Tests.Unit.CommandTests
{
    public class DeletePostCommandTests
    {
        private readonly Mock<IPostRepository> _postRepository;
        private readonly Mock<IResponseFactory> _responseFactory;
        private readonly Mock<IFileService> _fileService;
        private readonly DeletePostHandler _handler;

        public DeletePostCommandTests()
        {
            _postRepository = new Mock<IPostRepository>();
            _responseFactory = new Mock<IResponseFactory>();
            _fileService = new Mock<IFileService>();
            _handler = new DeletePostHandler(_postRepository.Object, _fileService.Object, _responseFactory.Object);
        }

        [Fact]
        public async Task Handle_Success()
        {
            const int IdToDelete = 2;
            var posts = new List<Post> 
            { 
                new Post { Id = 1, FileName = "post1" },
                new Post { Id = IdToDelete, FileName = "post2" },
                new Post { Id = 3, FileName = "post3" },
                new Post { Id = 4, FileName = "post4" },
            };

            _postRepository.Setup(x => x.DeleteByIdAsync(It.IsAny<long>()))
                .Callback((long id) => posts.Remove(posts.First(x => x.Id == id)));

            _postRepository.Setup(x => x.GetEntityByIdAsync(It.IsAny<long>()))
                .ReturnsAsync((long id) => posts.First(x => x.Id == id));

            _fileService.Setup(x => x.RemovePostImageAsync(It.IsAny<string>()));

            _responseFactory.Setup(x => x.CreateSuccess())
                .Returns(new ResponseModel { Success = true });

            var command = new DeletePostCommand { Id = IdToDelete };

            var res = await _handler.Handle(command, default);

            Assert.True(res.Success);
            Assert.DoesNotContain(posts, x => x.Id == command.Id);
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
                new Post { Id = 1, FileName = "post1" },
                new Post { Id = IdToDelete, FileName = "post2" },
                new Post { Id = 3, FileName = "post3" },
                new Post { Id = 4, FileName = "post4" },
            };
            const string Error = "Err";

            _postRepository.Setup(x => x.DeleteByIdAsync(It.IsAny<long>()))
                .Callback((long id) => throw new Exception(Error));

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
