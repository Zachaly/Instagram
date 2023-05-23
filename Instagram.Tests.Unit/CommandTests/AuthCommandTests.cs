using Instagram.Application.Abstraction;
using Instagram.Application.Command;
using Instagram.Database.Repository;
using Instagram.Domain.Entity;
using Instagram.Models.Response;
using Instagram.Models.User;
using Instagram.Models.User.Request;
using Moq;

namespace Instagram.Tests.Unit.CommandTests
{
    public class AuthCommandTests
    {
        private readonly Mock<IAuthService> _authService;
        private readonly Mock<IUserRepository> _userRepository;
        private readonly Mock<IUserFactory> _userFactory;
        private readonly Mock<IResponseFactory> _responseFactory;

        public AuthCommandTests()
        {
            _authService = new Mock<IAuthService>();
            _userRepository = new Mock<IUserRepository>();
            _userFactory = new Mock<IUserFactory>();
            _responseFactory = new Mock<IResponseFactory>();
        }

        [Fact]
        public async Task LoginCommand_Success()
        {
            var user = new User { Id = 1, Email = "mail" };
            const string Token = "token";

            _userRepository.Setup(x => x.GetEntityByEmailAsync(It.IsAny<string>())).ReturnsAsync(user);

            _authService.Setup(x => x.VerifyPasswordAsync(It.IsAny<string>(), It.IsAny<string>())).ReturnsAsync(true);

            _authService.Setup(x => x.GenerateTokenAsync(It.IsAny<User>())).ReturnsAsync(Token);

            _userFactory.Setup(x => x.CreateLoginResponse(It.IsAny<long>(), It.IsAny<string>(), It.IsAny<string>()))
                .Returns((long id, string token, string email) => new LoginResponse
                {
                    AuthToken = token,
                    Email = email,
                    UserId = id
                });

            _responseFactory.Setup(x => x.CreateSuccess(It.IsAny<LoginResponse>()))
                .Returns((LoginResponse data) => new DataResponseModel<LoginResponse> { Data = data, Success = true });

            var res = await new LoginHandler(_userRepository.Object, _authService.Object, _userFactory.Object, _responseFactory.Object)
                .Handle(new LoginCommand(), default);

            Assert.True(res.Success);
            Assert.Equal(Token, res.Data.AuthToken);
            Assert.Equal(user.Id, res.Data.UserId);
            Assert.Equal(user.Email, res.Data.Email);
        }

        [Fact]
        public async Task LoginCommand_InvalidPassword_Failure()
        {
            var user = new User { Id = 1, Email = "mail" };

            _userRepository.Setup(x => x.GetEntityByEmailAsync(It.IsAny<string>())).ReturnsAsync(user);

            _authService.Setup(x => x.VerifyPasswordAsync(It.IsAny<string>(), It.IsAny<string>())).ReturnsAsync(false);

            _responseFactory.Setup(x => x.CreateFailure<LoginResponse>(It.IsAny<string>()))
                .Returns((string error) => new DataResponseModel<LoginResponse> { Error = error, Success = false, Data = null });

            var res = await new LoginHandler(_userRepository.Object, _authService.Object, _userFactory.Object, _responseFactory.Object)
                .Handle(new LoginCommand(), default);

            Assert.False(res.Success);
            Assert.Null(res.Data);
            Assert.NotEmpty(res.Error);
        }

        [Fact]
        public async Task LoginCommand_UserNotFound_Failure()
        {
            _userRepository.Setup(x => x.GetEntityByEmailAsync(It.IsAny<string>())).ReturnsAsync(() => null);

            _responseFactory.Setup(x => x.CreateFailure<LoginResponse>(It.IsAny<string>()))
                .Returns((string error) => new DataResponseModel<LoginResponse> { Error = error, Success = false, Data = null });

            var res = await new LoginHandler(_userRepository.Object, _authService.Object, _userFactory.Object, _responseFactory.Object)
                .Handle(new LoginCommand(), default);

            Assert.False(res.Success);
            Assert.Null(res.Data);
            Assert.NotEmpty(res.Error);
        }

        [Fact]
        public async Task RegisterCommand_Success()
        {
            var users = new List<User>();
            const long UserId = 1;
            _userRepository.Setup(x => x.InsertAsync(It.IsAny<User>())).Callback((User user) => users.Add(user)).ReturnsAsync(UserId);
            _userRepository.Setup(x => x.GetEntityByEmailAsync(It.IsAny<string>()))
                .ReturnsAsync((string email) => users.FirstOrDefault(x => x.Email == email));

            const string Hash = "hash";
            _authService.Setup(x => x.HashPasswordAsync(It.IsAny<string>())).ReturnsAsync(Hash);

            _userFactory.Setup(x => x.Create(It.IsAny<RegisterRequest>(), It.IsAny<string>()))
                .Returns((RegisterRequest request, string passwordHash) => new User
                {
                    PasswordHash = passwordHash,
                    Nickname = request.Nickname
                });

            _responseFactory.Setup(x => x.CreateSuccess())
                .Returns(new ResponseModel { Success = true });

            var command = new RegisterCommand { Nickname = "nick", Email = "mail" };
            var res = await new RegisterHandler(_userFactory.Object, _userRepository.Object, _authService.Object, _responseFactory.Object)
                .Handle(command, default);

            Assert.True(res.Success);
            Assert.Contains(users, x => x.PasswordHash == Hash && x.Nickname == command.Nickname);
        }

        [Fact]
        public async Task RegisterCommand_UserExists_Fail()
        {
            _userRepository.Setup(x => x.GetEntityByEmailAsync(It.IsAny<string>()))
                .ReturnsAsync(new User());

            _responseFactory.Setup(x => x.CreateFailure(It.IsAny<string>()))
                .Returns((string error) => new ResponseModel { Success = false, Error = error });

            var command = new RegisterCommand { Nickname = "nick", Email = "mail" };
            var res = await new RegisterHandler(_userFactory.Object, _userRepository.Object, _authService.Object, _responseFactory.Object)
                .Handle(command, default);

            Assert.False(res.Success);
            Assert.NotEmpty(res.Error);
        }
    }
}
