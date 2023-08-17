using Instagram.Application.Abstraction;
using Instagram.Application.Command;
using Instagram.Database.Repository;
using Instagram.Domain.Entity;
using Instagram.Models.Post.Request;
using Instagram.Models.PostTag.Request;
using Instagram.Models.Response;
using Microsoft.AspNetCore.Http;
using NSubstitute;
using NSubstitute.ExceptionExtensions;

namespace Instagram.Tests.Unit.CommandTests
{
    public class AddPostCommandTests
    {
        private readonly IPostRepository _postRepository;
        private readonly IPostFactory _postFactory;
        private readonly IResponseFactory _responseFactory;
        private readonly IFileService _fileService;
        private readonly IPostImageRepository _postImageRepository;
        private readonly IPostTagService _postTagService;
        private readonly AddPostHandler _handler;

        public AddPostCommandTests()
        {
            _postRepository = Substitute.For<IPostRepository>();
            _postFactory = Substitute.For<IPostFactory>();
            _responseFactory = ResponseFactoryMock.Create();
            _fileService = Substitute.For<IFileService>();
            _postImageRepository = Substitute.For<IPostImageRepository>();
            _postTagService = Substitute.For<IPostTagService>();
            _handler = new AddPostHandler(_postFactory, _postRepository, _fileService,
                _responseFactory, _postImageRepository, _postTagService);
        }

        [Fact]
        public async Task Handle_Success()
        {
            var posts = new List<Post>();
            var images = new List<PostImage>();
            var tags = new List<string>();
            const string FileName = "file";
            const long PostId = 1;

            _fileService.SavePostImagesAsync(Arg.Any<IEnumerable<IFormFile>>())
                .Returns(new string[] { FileName });

            _postRepository.InsertAsync(Arg.Any<Post>())
                .Returns(PostId)
                .AndDoes(info => posts.Add(info.Arg<Post>()));

            _postFactory.Create(Arg.Any<AddPostRequest>())
                .Returns(info => new Post { CreatorId = info.Arg<AddPostRequest>().CreatorId });

            _postFactory.CreateImages(Arg.Any<IEnumerable<string>>(), Arg.Any<long>())
                .Returns(info => info.Arg<IEnumerable<string>>().Select(x => new PostImage { File = x, PostId = info.Arg<long>() }));

            _postImageRepository.InsertAsync(Arg.Any<PostImage>())
                .Returns(0)
                .AndDoes(info => images.Add(info.Arg<PostImage>()));

            _postTagService.AddAsync(Arg.Any<AddPostTagRequest>())
                .Returns(new ResponseModel { Success = true })
                .AndDoes(info => tags.AddRange(info.Arg<AddPostTagRequest>().Tags));

            var file = Substitute.For<IFormFile>();

            var command = new AddPostCommand
            {
                CreatorId = 1,
                Files = new IFormFile[] { file },
                Tags = new string[] { "tag1", "tag2", "tag3" }
            };

            var res = await _handler.Handle(command, default);

            Assert.True(res.Success);
            Assert.Contains(posts, x => x.CreatorId == command.CreatorId);
            Assert.Contains(images, x => x.PostId == PostId && x.File == FileName);
            Assert.Single(images);
            Assert.Equivalent(command.Tags, tags);
        }

        [Fact]
        public async Task Handle_FileNull_Fail()
        {
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

            _fileService.SavePostImagesAsync(Arg.Any<IEnumerable<IFormFile>>())
                .ThrowsAsync(new Exception(Error));

            var file = Substitute.For<IFormFile>();

            var command = new AddPostCommand
            {
                CreatorId = 1,
                Files = new IFormFile[] { file },
            };

            var res = await _handler.Handle(command, default);

            Assert.False(res.Success);
            Assert.Equal(Error, res.Error);
        }
    }
}
