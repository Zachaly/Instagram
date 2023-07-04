using Instagram.Application.Abstraction;
using Instagram.Application.Command;
using Instagram.Database.Repository;
using Instagram.Domain.Entity;
using Instagram.Models.Response;
using Instagram.Models.User.Request;
using Moq;

namespace Instagram.Tests.Unit.CommandTests
{
    public class ChangePasswordCommandTests
    {
        private readonly Mock<IAuthService> _authService;
        private readonly Mock<IUserRepository> _userRepository;
        private readonly Mock<IResponseFactory> _responseFactory;
        private readonly ChangePasswordHandler _handler;

        public ChangePasswordCommandTests()
        {
            _authService = new Mock<IAuthService>();
            _userRepository = new Mock<IUserRepository>();
            _responseFactory = new Mock<IResponseFactory>();

            _responseFactory.Setup(x => x.CreateSuccess())
                .Returns(new ResponseModel { Success = true });

            _responseFactory.Setup(x => x.CreateFailure(It.IsAny<string>()))
                .Returns((string err) => new ResponseModel { Success = false, Error = err });

            _handler = new ChangePasswordHandler(_authService.Object, _userRepository.Object, _responseFactory.Object);
        }

        [Fact]
        public async Task Handle_Success()
        {
            var user = new User();

            _userRepository.Setup(x => x.GetEntityByIdAsync(It.IsAny<long>()))
                .ReturnsAsync(user);

            _userRepository.Setup(x => x.UpdateAsync(It.IsAny<UpdateUserRequest>()))
                .Callback((UpdateUserRequest request) => user.PasswordHash = request.PasswordHash);

            _authService.Setup(x => x.VerifyPasswordAsync(It.IsAny<string>(), It.IsAny<string>()))
                .ReturnsAsync(true);

            const string Hash = "hash";

            _authService.Setup(x => x.HashPasswordAsync(It.IsAny<string>()))
                .ReturnsAsync(Hash);

            var command = new ChangePasswordCommand();

            var res = await _handler.Handle(command, default);

            Assert.True(res.Success);
            Assert.Equal(Hash, user.PasswordHash);
        }

        [Fact]
        public async Task Handle_UserNotFound_Failure()
        {
            _userRepository.Setup(x => x.GetEntityByIdAsync(It.IsAny<long>()))
                .ReturnsAsync(() => null);

            var command = new ChangePasswordCommand();

            var res = await _handler.Handle(command, default);

            Assert.False(res.Success);
            Assert.NotEmpty(res.Error);
        }

        [Fact]
        public async Task Handle_InvalidOldPassword_Failure()
        {
            _userRepository.Setup(x => x.GetEntityByIdAsync(It.IsAny<long>()))
                .ReturnsAsync(() => new User());

            _authService.Setup(x => x.VerifyPasswordAsync(It.IsAny<string>(), It.IsAny<string>()))
                .ReturnsAsync(false);

            var command = new ChangePasswordCommand();

            var res = await _handler.Handle(command, default);

            Assert.False(res.Success);
            Assert.NotEmpty(res.Error);
        }

        [Fact]
        public async Task Handle_ExceptionThrown_Failure()
        {
            _userRepository.Setup(x => x.GetEntityByIdAsync(It.IsAny<long>()))
                .ReturnsAsync(() => new User());

            _authService.Setup(x => x.VerifyPasswordAsync(It.IsAny<string>(), It.IsAny<string>()))
                .ReturnsAsync(true);

            const string Error = "err";
            _authService.Setup(x => x.HashPasswordAsync(It.IsAny<string>()))
                .Callback(() => throw new Exception(Error));

            var command = new ChangePasswordCommand();

            var res = await _handler.Handle(command, default);

            Assert.False(res.Success);
            Assert.Equal(Error, res.Error);
        }
    }
}
