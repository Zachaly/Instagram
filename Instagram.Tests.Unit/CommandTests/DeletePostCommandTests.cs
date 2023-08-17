using Instagram.Application.Abstraction;
using Instagram.Application.Command;
using Instagram.Database.Repository;
using Instagram.Domain.Entity;
using Instagram.Models.PostImage;
using Instagram.Models.PostImage.Request;
using NSubstitute;
using NSubstitute.ExceptionExtensions;
using NSubstitute.ReturnsExtensions;

namespace Instagram.Tests.Unit.CommandTests
{
    public class DeletePostCommandTests
    {
        private readonly IPostRepository _postRepository;
        private readonly IResponseFactory _responseFactory;
        private readonly IFileService _fileService;
        private readonly IPostImageRepository _postImageRepository;
        private readonly IPostTagRepository _postTagRepository;
        private readonly DeletePostHandler _handler;

        public DeletePostCommandTests()
        {
            _postRepository = Substitute.For<IPostRepository>();
            _responseFactory = ResponseFactoryMock.Create();
            _fileService = Substitute.For<IFileService>();
            _postImageRepository = Substitute.For<IPostImageRepository>();
            _postTagRepository = Substitute.For<IPostTagRepository>();
            _handler = new DeletePostHandler(_postRepository, _fileService, _responseFactory,
                _postImageRepository, _postTagRepository);
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

            _postRepository.DeleteByIdAsync(Arg.Any<long>())
                .Returns(Task.CompletedTask)
                .AndDoes(info => posts.RemoveAll(p => p.Id == info.Arg<long>()));

            _postRepository.GetEntityByIdAsync(Arg.Any<long>()).Returns(info => posts.First(p => p.Id == info.Arg<long>()));

            _postImageRepository.DeleteByPostIdAsync(Arg.Any<long>())
                .Returns(Task.CompletedTask)
                .AndDoes(info => images.RemoveAll(p => p.PostId == info.Arg<long>()));

            _postImageRepository.GetAsync(Arg.Any<GetPostImageRequest>())
                .Returns(info =>
                    images.Where(img => img.PostId == info.Arg<GetPostImageRequest>().PostId!)
                    .Select(img => new PostImageModel { File = img.File }));

            _fileService.RemovePostImageAsync(Arg.Any<string>()).Returns(Task.CompletedTask);

            _postTagRepository.DeleteByPostIdAsync(Arg.Any<long>())
                .Returns(Task.CompletedTask)
                .AndDoes(info => tags.RemoveAll(t => t.PostId == info.Arg<long>()));

            var command = new DeletePostCommand { Id = IdToDelete };

            var res = await _handler.Handle(command, default);

            Assert.True(res.Success);
            Assert.DoesNotContain(posts, x => x.Id == command.Id);
            Assert.DoesNotContain(images, x => x.PostId == command.Id);
            Assert.DoesNotContain(tags, x => x.PostId == command.Id);
            Assert.Equal(2, _fileService.GetMethodCallsNumber(nameof(_fileService.RemovePostImageAsync)));
        }

        [Fact]
        public async Task Handle_PostNotFound_Fail()
        {
            _postRepository.GetEntityByIdAsync(Arg.Any<long>()).ReturnsNull();

            var command = new DeletePostCommand { Id = 2137 };

            var res = await _handler.Handle(command, default);

            Assert.False(res.Success);
            Assert.NotEmpty(res.Error);
        }

        [Fact]
        public async Task Handle_ExceptionThrown_Fail()
        {
            const string Error = "Err";

            _postRepository.GetEntityByIdAsync(Arg.Any<long>()).ThrowsAsync(new Exception(Error));

            var command = new DeletePostCommand { Id = 2137 };

            var res = await _handler.Handle(command, default);

            Assert.False(res.Success);
            Assert.Equal(Error, res.Error);
        }
    }
}
