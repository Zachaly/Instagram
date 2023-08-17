using Instagram.Application.Abstraction;
using Instagram.Application.Command;
using Instagram.Database.Repository;
using Instagram.Domain.Entity;
using Instagram.Models.User.Request;
using NSubstitute;
using NSubstitute.ExceptionExtensions;
using NSubstitute.ReturnsExtensions;

namespace Instagram.Tests.Unit.CommandTests
{
    public class ChangePasswordCommandTests
    {
        private readonly IAuthService _authService;
        private readonly IUserRepository _userRepository;
        private readonly IResponseFactory _responseFactory;
        private readonly ChangePasswordHandler _handler;

        public ChangePasswordCommandTests()
        {
            _authService = Substitute.For<IAuthService>();
            _userRepository = Substitute.For<IUserRepository>();
            _responseFactory = ResponseFactoryMock.Create();

            _handler = new ChangePasswordHandler(_authService, _userRepository, _responseFactory);
        }

        [Fact]
        public async Task Handle_Success()
        {
            var user = new User();

            _userRepository.GetEntityByIdAsync(Arg.Any<long>()).Returns(user);

            _userRepository.UpdateAsync(Arg.Any<UpdateUserRequest>())
                .Returns(Task.CompletedTask)
                .AndDoes(info => user.PasswordHash = info.Arg<UpdateUserRequest>().PasswordHash);

            _authService.VerifyPasswordAsync(Arg.Any<string>(), Arg.Any<string>()).Returns(true);

            const string Hash = "hash";

            _authService.HashPasswordAsync(Arg.Any<string>()).Returns(Hash);

            var command = new ChangePasswordCommand();

            var res = await _handler.Handle(command, default);

            Assert.True(res.Success);
            Assert.Equal(Hash, user.PasswordHash);
        }

        [Fact]
        public async Task Handle_UserNotFound_Failure()
        {
            _userRepository.GetEntityByIdAsync(Arg.Any<long>()).ReturnsNull();

            var command = new ChangePasswordCommand();

            var res = await _handler.Handle(command, default);

            Assert.False(res.Success);
            Assert.NotEmpty(res.Error);
        }

        [Fact]
        public async Task Handle_InvalidOldPassword_Failure()
        {
            _userRepository.GetEntityByIdAsync(Arg.Any<long>()).Returns(new User());

            _authService.VerifyPasswordAsync(Arg.Any<string>(), Arg.Any<string>()).Returns(false);

            var command = new ChangePasswordCommand();

            var res = await _handler.Handle(command, default);

            Assert.False(res.Success);
            Assert.NotEmpty(res.Error);
        }

        [Fact]
        public async Task Handle_ExceptionThrown_Failure()
        {
            const string Error = "err";
            _userRepository.GetEntityByIdAsync(Arg.Any<long>()).ThrowsAsync(new Exception(Error));
            
            var command = new ChangePasswordCommand();

            var res = await _handler.Handle(command, default);

            Assert.False(res.Success);
            Assert.Equal(Error, res.Error);
        }
    }
}
