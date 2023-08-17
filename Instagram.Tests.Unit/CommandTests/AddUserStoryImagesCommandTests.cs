using Instagram.Application.Abstraction;
using Instagram.Application.Command;
using Instagram.Database.Repository;
using Instagram.Domain.Entity;
using Instagram.Models.Response;
using Microsoft.AspNetCore.Http;
using NSubstitute;
using NSubstitute.ExceptionExtensions;

namespace Instagram.Tests.Unit.CommandTests
{
    public class AddUserStoryImagesCommandTests
    {
        private readonly IUserStoryImageRepository _userStoryRepository;
        private readonly IUserStoryFactory _userStoryFactory;
        private readonly IResponseFactory _responseFactory;
        private readonly IFileService _fileService;
        private readonly AddUserStoryImagesHandler _handler;

        public AddUserStoryImagesCommandTests()
        {
            _userStoryRepository = Substitute.For<IUserStoryImageRepository>();
            _userStoryFactory = Substitute.For<IUserStoryFactory>();
            _responseFactory = ResponseFactoryMock.Create();
            _fileService = Substitute.For<IFileService>();

            _handler = new AddUserStoryImagesHandler(_userStoryRepository, _userStoryFactory,
                _responseFactory, _fileService);
        }

        [Fact]
        public async Task Handle_AddsImages()
        {
            var images = new List<UserStoryImage>();

            _userStoryRepository.InsertAsync(Arg.Any<UserStoryImage>())
                .Returns(0)
                .AndDoes(info => images.Add(info.Arg<UserStoryImage>()));

            _userStoryFactory.Create(Arg.Any<long>(), Arg.Any<string>())
                .Returns(info => new UserStoryImage
                    {
                        UserId = info.Arg<long>(),
                        FileName = info.Arg<string>(),
                    });

            _fileService.SaveStoryImagesAsync(Arg.Any<IEnumerable<IFormFile>>())
                .Returns(info => info.Arg<IEnumerable<IFormFile>>().Select(f => f.Name));

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
                    var mock = Substitute.For<IFormFile>();
                    mock.Name.Returns(name);

                    return mock;
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

            _fileService.SaveStoryImagesAsync(Arg.Any<IEnumerable<IFormFile>>())
                .ThrowsAsync(new Exception(Error));

            var res = await _handler.Handle(new AddUserStoryImagesCommand(), default);

            Assert.False(res.Success);
            Assert.Equal(Error, res.Error);
        }
    }
}
