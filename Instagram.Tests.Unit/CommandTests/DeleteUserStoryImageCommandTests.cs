using Instagram.Application.Abstraction;
using Instagram.Application.Command;
using Instagram.Database.Repository;
using Instagram.Domain.Entity;
using Instagram.Models.Response;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;

namespace Instagram.Tests.Unit.CommandTests
{
    public class DeleteUserStoryImageCommandTests
    {
        private readonly Mock<IUserStoryImageRepository> _userStoryRepository;
        private readonly Mock<IResponseFactory> _responseFactory;
        private readonly Mock<IFileService> _fileService;
        private readonly DeleteUserStoryImageHandler _handler;

        public DeleteUserStoryImageCommandTests()
        {
            _userStoryRepository = new Mock<IUserStoryImageRepository>();
            _responseFactory = new Mock<IResponseFactory>();
            _fileService = new Mock<IFileService>();

            _responseFactory.Setup(x => x.CreateSuccess())
                .Returns(new ResponseModel { Success = true });
            _responseFactory.Setup(x => x.CreateFailure(It.IsAny<string>()))
                .Returns((string err) => new ResponseModel { Success = false, Error = err });

            _handler = new DeleteUserStoryImageHandler(_userStoryRepository.Object, _fileService.Object,
                _responseFactory.Object);
        }

        [Fact]
        public async Task Handle_DeletesImage()
        {
            const long Id = 3;

            var images = new List<UserStoryImage>
            {
                new UserStoryImage { Id = 1, },
                new UserStoryImage { Id = 2, },
                new UserStoryImage { Id = Id, },
            };

            var fileDeleted = false;

            _userStoryRepository.Setup(x => x.DeleteByIdAsync(It.IsAny<long>()))
                .Callback((long id) => images.RemoveAll(x => x.Id == id));

            _userStoryRepository.Setup(x => x.GetEntityByIdAsync(It.IsAny<long>()))
                .ReturnsAsync((long id) => images.First(x => x.Id == id));

            _fileService.Setup(x => x.RemoveStoryImageAsync(It.IsAny<string>()))
                .Callback(() => fileDeleted = true);

            var command = new DeleteUserStoryImageCommand { Id = Id };

            var res = await _handler.Handle(command, default);

            Assert.True(res.Success);
            Assert.DoesNotContain(images, x => x.Id == command.Id);
            Assert.True(fileDeleted);
        }

        [Fact]
        public async Task Handle_ExceptionThrown_Failure()
        {
            const string Error = "err";

            _userStoryRepository.Setup(x => x.GetEntityByIdAsync(It.IsAny<long>()))
                .ThrowsAsync(new Exception(Error));

            var res = await _handler.Handle(new DeleteUserStoryImageCommand(), default);

            Assert.False(res.Success);
            Assert.Equal(Error, res.Error);
        }
    }
}
