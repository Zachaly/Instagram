using Instagram.Application.Abstraction;
using Instagram.Application.Command;
using Instagram.Database.Repository;
using Instagram.Domain.Entity;
using Instagram.Models.User.Request;
using Microsoft.AspNetCore.Http;
using NSubstitute;
using NSubstitute.ExceptionExtensions;
using NSubstitute.ReturnsExtensions;

namespace Instagram.Tests.Unit.CommandTests
{
    public class UpdateProfilePictureCommandTests
    {
        private readonly IFileService _fileService;
        private readonly IUserRepository _userRepository;
        private readonly IResponseFactory _responseFactory;

        private readonly UpdateProfilePictureHandler _handler;

        public UpdateProfilePictureCommandTests()
        {
            _fileService = Substitute.For<IFileService>();
            _userRepository = Substitute.For<IUserRepository>();
            _responseFactory = ResponseFactoryMock.Create();

            _handler = new UpdateProfilePictureHandler(_fileService, _userRepository, _responseFactory);
        }

        [Fact]
        public async Task Handle_UserWithNoPicture_Success()
        {
            const string NewFileName = "file";

            _fileService.SaveProfilePictureAsync(Arg.Any<IFormFile>()).Returns(NewFileName);

            var user = new User { ProfilePicture = null };

            _userRepository.GetEntityByIdAsync(Arg.Any<long>())
                .Returns(user);

            _userRepository.UpdateAsync(Arg.Any<UpdateUserRequest>())
                .Returns(Task.CompletedTask)
                .AndDoes(info => user.ProfilePicture = info.Arg<UpdateUserRequest>().ProfilePicture);

            var file = Substitute.For<IFormFile>();

            var command = new UpdateProfilePictureCommand { UserId = 1, File = file };

            var res = await _handler.Handle(command, default);

            Assert.True(res.Success);
            Assert.Equal(NewFileName, user.ProfilePicture);
        }

        [Fact]
        public async Task Handle_UserWithPicture_PictureReplaced_Success()
        {
            const string NewFileName = "file";

            _fileService.RemoveProfilePictureAsync(Arg.Any<string>()).Returns(Task.CompletedTask);
            _fileService.SaveProfilePictureAsync(Arg.Any<IFormFile>()).Returns(NewFileName);

            var user = new User { ProfilePicture = "prof" };

            _userRepository.GetEntityByIdAsync(Arg.Any<long>())
                .Returns(user);

            _userRepository.UpdateAsync(Arg.Any<UpdateUserRequest>())
                .Returns(Task.CompletedTask)
                .AndDoes(info => user.ProfilePicture = info.Arg<UpdateUserRequest>().ProfilePicture);

            var file = Substitute.For<IFormFile>();

            var command = new UpdateProfilePictureCommand { UserId = 1, File = file };

            var res = await _handler.Handle(command, default);

            Assert.True(res.Success);
            Assert.Equal(1, _fileService.GetMethodCallsNumber(nameof(_fileService.RemoveProfilePictureAsync)));
            Assert.Equal(NewFileName, user.ProfilePicture);
        }

        [Fact]
        public async Task Handle_UserNotFound_Fail()
        {
            _userRepository.GetEntityByIdAsync(Arg.Any<long>()).ReturnsNull();

            var file = Substitute.For<IFormFile>();

            var command = new UpdateProfilePictureCommand { UserId = 1, File = file };

            var res = await _handler.Handle(command, default);

            Assert.False(res.Success);
        }

        [Fact]
        public async Task Handle_ExceptionThrown_Fail()
        {
            const string Error = "error";

            _userRepository.GetEntityByIdAsync(Arg.Any<long>()).ThrowsAsync(new Exception(Error));

            var file = Substitute.For<IFormFile>();

            var command = new UpdateProfilePictureCommand { UserId = 1, File = file };

            var res = await _handler.Handle(command, default);

            Assert.False(res.Success);
            Assert.Equal(Error, res.Error);
        }
    }
}
