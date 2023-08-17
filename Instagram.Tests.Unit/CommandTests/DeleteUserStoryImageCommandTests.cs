using Instagram.Application.Abstraction;
using Instagram.Application.Command;
using Instagram.Database.Repository;
using Instagram.Domain.Entity;
using Instagram.Models.Response;
using NSubstitute;
using NSubstitute.ExceptionExtensions;

namespace Instagram.Tests.Unit.CommandTests
{
    public class DeleteUserStoryImageCommandTests
    {
        private readonly IUserStoryImageRepository _userStoryRepository;
        private readonly IResponseFactory _responseFactory;
        private readonly IFileService _fileService;
        private readonly DeleteUserStoryImageHandler _handler;

        public DeleteUserStoryImageCommandTests()
        {
            _userStoryRepository = Substitute.For<IUserStoryImageRepository>();
            _responseFactory = ResponseFactoryMock.Create();
            _fileService = Substitute.For<IFileService>();

            _handler = new DeleteUserStoryImageHandler(_userStoryRepository, _fileService, _responseFactory);
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

            _userStoryRepository.DeleteByIdAsync(Arg.Any<long>())
                .Returns(Task.CompletedTask)
                .AndDoes(info => images.RemoveAll(img => img.Id == info.Arg<long>()));

            _userStoryRepository.GetEntityByIdAsync(Arg.Any<long>())
                .Returns(info => images.First(img => img.Id == info.Arg<long>()));

            _fileService.RemoveStoryImageAsync(Arg.Any<string>()).Returns(Task.CompletedTask);

            var command = new DeleteUserStoryImageCommand { Id = Id };

            var res = await _handler.Handle(command, default);

            Assert.True(res.Success);
            Assert.DoesNotContain(images, x => x.Id == command.Id);
            Assert.Equal(1, _fileService.GetMethodCallsNumber(nameof(_fileService.RemoveStoryImageAsync)));
        }

        [Fact]
        public async Task Handle_ExceptionThrown_Failure()
        {
            const string Error = "err";

            _userStoryRepository.GetEntityByIdAsync(Arg.Any<long>()).ThrowsAsync(new Exception(Error));

            var res = await _handler.Handle(new DeleteUserStoryImageCommand(), default);

            Assert.False(res.Success);
            Assert.Equal(Error, res.Error);
        }
    }
}
