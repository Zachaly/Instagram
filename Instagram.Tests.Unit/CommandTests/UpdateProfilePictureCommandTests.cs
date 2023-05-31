using Instagram.Application.Abstraction;
using Instagram.Application.Command;
using Instagram.Database.Repository;
using Instagram.Domain.Entity;
using Instagram.Models.Response;
using Instagram.Models.User.Request;
using Microsoft.AspNetCore.Http;
using Moq;

namespace Instagram.Tests.Unit.CommandTests
{
    public class UpdateProfilePictureCommandTests
    {
        private readonly Mock<IFileService> _fileService;
        private readonly Mock<IUserRepository> _userRepository;
        private readonly Mock<IResponseFactory> _responseFactory;

        private readonly UpdateProfilePictureHandler _handler;

        public UpdateProfilePictureCommandTests()
        {
            _fileService = new Mock<IFileService>();
            _userRepository = new Mock<IUserRepository>();
            _responseFactory = new Mock<IResponseFactory>();

            _handler = new UpdateProfilePictureHandler(_fileService.Object, _userRepository.Object, _responseFactory.Object);
        }

        [Fact]
        public async Task Handle_UserWithNoPicture_Success()
        {
            const string NewFileName = "file";

            _fileService.Setup(x => x.SaveProfilePictureAsync(It.IsAny<IFormFile>())).ReturnsAsync(NewFileName);

            var user = new User { ProfilePicture = null };

            _userRepository.Setup(x => x.GetEntityByIdAsync(It.IsAny<long>()))
                .ReturnsAsync(user);

            _userRepository.Setup(x => x.UpdateAsync(It.IsAny<UpdateUserRequest>()))
                .Callback((UpdateUserRequest request) =>
                {
                    user.ProfilePicture = request.ProfilePicture;
                });

            _responseFactory.Setup(x => x.CreateSuccess()).Returns(new ResponseModel { Success = true });

            var file = new Mock<IFormFile>();

            var command = new UpdateProfilePictureCommand { UserId = 1, File = file.Object };

            var res = await _handler.Handle(command, default);

            Assert.True(res.Success);
            Assert.Equal(NewFileName, user.ProfilePicture);
        }

        [Fact]
        public async Task Handle_UserWithPicture_PictureReplaced_Success()
        {
            const string NewFileName = "file";

            _fileService.Setup(x => x.RemoveProfilePictureAsync(It.IsAny<string>()));
            _fileService.Setup(x => x.SaveProfilePictureAsync(It.IsAny<IFormFile>())).ReturnsAsync(NewFileName);

            var user = new User { ProfilePicture = "prof" };

            _userRepository.Setup(x => x.GetEntityByIdAsync(It.IsAny<long>()))
                .ReturnsAsync(user);

            _userRepository.Setup(x => x.UpdateAsync(It.IsAny<UpdateUserRequest>()))
                .Callback((UpdateUserRequest request) =>
                {
                    user.ProfilePicture = request.ProfilePicture;
                });

            _responseFactory.Setup(x => x.CreateSuccess()).Returns(new ResponseModel { Success = true });

            var file = new Mock<IFormFile>();

            var command = new UpdateProfilePictureCommand { UserId = 1, File = file.Object };

            var res = await _handler.Handle(command, default);

            Assert.True(res.Success);
            Assert.Equal(NewFileName, user.ProfilePicture);
        }

        [Fact]
        public async Task Handle_UserNotFound_Fail()
        {
            _userRepository.Setup(x => x.GetEntityByIdAsync(It.IsAny<long>()))
                .ReturnsAsync(() => null);

            _responseFactory.Setup(x => x.CreateFailure(It.IsAny<string>())).Returns(new ResponseModel { Success = false });

            var file = new Mock<IFormFile>();

            var command = new UpdateProfilePictureCommand { UserId = 1, File = file.Object };

            var res = await _handler.Handle(command, default);

            Assert.False(res.Success);
        }

        [Fact]
        public async Task Handle_ExceptionThrown_Fail()
        {
            const string Error = "error";

            _fileService.Setup(x => x.RemoveProfilePictureAsync(It.IsAny<string>()))
                .Callback(() => throw new Exception(Error));

            var user = new User { ProfilePicture = "prof" };

            _userRepository.Setup(x => x.GetEntityByIdAsync(It.IsAny<long>()))
                .ReturnsAsync(user);

            _responseFactory.Setup(x => x.CreateFailure(It.IsAny<string>()))
                .Returns((string error) => new ResponseModel { Success = false, Error = error });

            var file = new Mock<IFormFile>();

            var command = new UpdateProfilePictureCommand { UserId = 1, File = file.Object };

            var res = await _handler.Handle(command, default);

            Assert.False(res.Success);
            Assert.Equal(Error, res.Error);
        }
    }
}
