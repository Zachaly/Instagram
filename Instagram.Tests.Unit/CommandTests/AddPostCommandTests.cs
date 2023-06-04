using Instagram.Application.Abstraction;
using Instagram.Application.Command;
using Instagram.Database.Repository;
using Instagram.Domain.Entity;
using Instagram.Models.Post.Request;
using Instagram.Models.Response;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Features;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

            _fileService.Setup(x => x.SavePostImageAsync(It.IsAny<IFormFile>())).ReturnsAsync(FileName);

            _postRepository.Setup(x => x.InsertAsync(It.IsAny<Post>()))
                .Callback((Post post) => posts.Add(post));

            _postFactory.Setup(x => x.Create(It.IsAny<AddPostRequest>(), It.IsAny<string>()))
                .Returns((AddPostRequest request, string file) => new Post { CreatorId = request.CreatorId, FileName = file });

            _responseFactory.Setup(x => x.CreateSuccess())
                .Returns(new ResponseModel { Success = true });

            var file = new Mock<IFormFile>();

            var command = new AddPostCommand
            {
                CreatorId = 1,
                File = file.Object,
            };

            var res = await _handler.Handle(command, default);

            Assert.True(res.Success);
            Assert.Contains(posts, x => x.CreatorId == command.CreatorId && x.FileName == FileName);
        }

        [Fact]
        public async Task Handle_FileNull_Fail()
        {
            _responseFactory.Setup(x => x.CreateFailure(It.IsAny<string>()))
                .Returns((string error) => new ResponseModel { Success = false, Error = error });

            var command = new AddPostCommand
            {
                CreatorId = 1,
                File = null,
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

            _fileService.Setup(x => x.SavePostImageAsync(It.IsAny<IFormFile>()))
                .Callback(() => throw new Exception(Error));

            _responseFactory.Setup(x => x.CreateFailure(It.IsAny<string>()))
                .Returns((string error) => new ResponseModel { Success = false, Error = error });

            var file = new Mock<IFormFile>();

            var command = new AddPostCommand
            {
                CreatorId = 1,
                File = file.Object,
            };

            var res = await _handler.Handle(command, default);

            Assert.False(res.Success);
            Assert.Equal(Error, res.Error);
        }
    }
}
