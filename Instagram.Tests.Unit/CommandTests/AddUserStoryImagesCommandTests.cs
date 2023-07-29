using Instagram.Application.Abstraction;
using Instagram.Application.Command;
using Instagram.Database.Repository;
using Instagram.Domain.Entity;
using Instagram.Models.Response;
using Microsoft.AspNetCore.Http;
using Moq;

namespace Instagram.Tests.Unit.CommandTests
{
    public class AddUserStoryImagesCommandTests
    {
        private readonly Mock<IUserStoryImageRepository> _userStoryRepository;
        private readonly Mock<IUserStoryFactory> _userStoryFactory;
        private readonly Mock<IResponseFactory> _responseFactory;
        private readonly Mock<IFileService> _fileService;
        private readonly AddUserStoryImagesHandler _handler;

        public AddUserStoryImagesCommandTests()
        {
            _userStoryRepository = new Mock<IUserStoryImageRepository>();
            _userStoryFactory = new Mock<IUserStoryFactory>();
            _responseFactory = new Mock<IResponseFactory>();
            _fileService = new Mock<IFileService>();

            _responseFactory.Setup(x => x.CreateSuccess())
                .Returns(new ResponseModel { Success = true });

            _responseFactory.Setup(x => x.CreateFailure(It.IsAny<string>()))
                .Returns((string err) => new ResponseModel { Success = false, Error = err });

            _handler = new AddUserStoryImagesHandler(_userStoryRepository.Object, _userStoryFactory.Object,
                _responseFactory.Object, _fileService.Object);
        }

        [Fact]
        public async Task Handle_AddsImages()
        {
            var images = new List<UserStoryImage>();

            _userStoryRepository.Setup(x => x.InsertAsync(It.IsAny<UserStoryImage>()))
                .Callback((UserStoryImage image) => images.Add(image));

            _userStoryFactory.Setup(x => x.Create(It.IsAny<long>(), It.IsAny<string>()))
                .Returns((long id, string fileName) => new UserStoryImage { UserId = id, FileName = fileName });

            _fileService.Setup(x => x.SaveStoryImagesAsync(It.IsAny<IEnumerable<IFormFile>>()))
                .ReturnsAsync((IEnumerable<IFormFile> files) => files.Select(x => x.Name));

            var fileNames = new List<string>
            {
                "file1",
                "file2"
            };

            var command = new AddUserStoryImagesCommand
            {
                UserId = 1,
                Images = fileNames.Select(name =>
                {
                    var mock = new Mock<IFormFile>();
                    mock.Setup(x => x.Name).Returns(name);

                    return mock.Object;
                })
            };

            var res = await _handler.Handle(command, default);

            Assert.True(res.Success);
            Assert.Equivalent(fileNames, images.Select(x => x.FileName));
            Assert.All(images, img =>
            {
                Assert.Equal(command.UserId, img.UserId);
            });
        }

        [Fact]
        public async Task Handle_ExceptionThrown_Failure()
        {
            const string Error = "err";

            _fileService.Setup(x => x.SaveStoryImagesAsync(It.IsAny<IEnumerable<IFormFile>>()))
                .ThrowsAsync(new Exception(Error));

            var res = await _handler.Handle(new AddUserStoryImagesCommand(), default);

            Assert.False(res.Success);
            Assert.Equal(Error, res.Error);
        }
    }
}
