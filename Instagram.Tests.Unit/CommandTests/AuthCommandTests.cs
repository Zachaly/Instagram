using Instagram.Application.Abstraction;
using Instagram.Application.Command;
using Instagram.Database.Repository;
using Instagram.Domain.Entity;
using Instagram.Models.Response;
using Instagram.Models.User;
using Instagram.Models.User.Request;
using Instagram.Models.UserClaim.Request;
using NSubstitute;
using NSubstitute.ReturnsExtensions;

namespace Instagram.Tests.Unit.CommandTests
{
    public class AuthCommandTests
    {
        private readonly IAuthService _authService;
        private readonly IUserRepository _userRepository;
        private readonly IUserFactory _userFactory;
        private readonly IResponseFactory _responseFactory;
        private readonly IUserClaimRepository _userClaimRepository;

        public AuthCommandTests()
        {
            _authService = Substitute.For<IAuthService>();
            _userRepository = Substitute.For<IUserRepository>();
            _userFactory = Substitute.For<IUserFactory>();
            _responseFactory = ResponseFactoryMock.Create();
            _userClaimRepository = Substitute.For<IUserClaimRepository>();
        }

        [Fact]
        public async Task LoginCommand_Success()
        {
            var user = new User { Id = 1, Email = "mail" };
            const string Token = "token";

            _userRepository.GetEntityByEmailAsync(Arg.Any<string>()).Returns(user);

            _authService.VerifyPasswordAsync(Arg.Any<string>(), Arg.Any<string>()).Returns(true);

            _authService.GenerateTokenAsync(Arg.Any<User>(), Arg.Any<IEnumerable<UserClaim>>()).Returns(Token);

            _userFactory.CreateLoginResponse(Arg.Any<long>(), Arg.Any<string>(), Arg.Any<string>(), Arg.Any<IEnumerable<UserClaim>>())
                .Returns(info =>
                    new LoginResponse
                    {
                        AuthToken = info.ArgAt<string>(1),
                        Email = info.ArgAt<string>(2),
                        UserId = info.Arg<long>(),
                    });

            _userClaimRepository.GetEntitiesAsync(Arg.Any<GetUserClaimRequest>())
                .Returns(Enumerable.Empty<UserClaim>());

            _responseFactory.CreateSuccess(Arg.Any<LoginResponse>())
                .Returns(info => new DataResponseModel<LoginResponse> { Data = info.Arg<LoginResponse>(), Success = true });

            var res = await new LoginHandler(_userRepository, _authService,
                _userFactory, _responseFactory, _userClaimRepository)
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

            _userRepository.GetEntityByEmailAsync(Arg.Any<string>()).Returns(user);

            _authService.VerifyPasswordAsync(Arg.Any<string>(), Arg.Any<string>()).Returns(false);

            _responseFactory.CreateFailure<LoginResponse>(Arg.Any<string>())
                .Returns(info => new DataResponseModel<LoginResponse>
                {
                    Error = info.Arg<string>(),
                    Success = false,
                    Data = null
                });

            var res = await new LoginHandler(_userRepository, _authService,
                _userFactory, _responseFactory, _userClaimRepository)
                .Handle(new LoginCommand(), default);

            Assert.False(res.Success);
            Assert.Null(res.Data);
            Assert.NotEmpty(res.Error);
        }

        [Fact]
        public async Task LoginCommand_UserNotFound_Failure()
        {
            _userRepository.GetEntityByEmailAsync(Arg.Any<string>()).ReturnsNull();

            _responseFactory.CreateFailure<LoginResponse>(Arg.Any<string>())
                .Returns(info => new DataResponseModel<LoginResponse>
                {
                    Error = info.Arg<string>(),
                    Success = false,
                    Data = null
                });

            var res = await new LoginHandler(_userRepository, _authService, _userFactory,
                _responseFactory, _userClaimRepository)
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

            _userRepository.InsertAsync(Arg.Any<User>())
                .Returns(UserId)
                .AndDoes(info => users.Add(info.Arg<User>()));

            _userRepository.GetEntityByEmailAsync(Arg.Any<string>())
                .ReturnsNull();

            const string Hash = "hash";
            _authService.HashPasswordAsync(Arg.Any<string>()).Returns(Hash);

            _userFactory.Create(Arg.Any<RegisterRequest>(), Arg.Any<string>())
                .Returns(info => new User
                {
                    Nickname = info.Arg<RegisterRequest>().Nickname,
                    PasswordHash = info.Arg<string>()
                });

            var command = new RegisterCommand { Nickname = "nick", Email = "mail" };
            var res = await new RegisterHandler(_userFactory, _userRepository, _authService, _responseFactory)
                .Handle(command, default);

            Assert.True(res.Success);
            Assert.Contains(users, x => x.PasswordHash == Hash && x.Nickname == command.Nickname);
        }

        [Fact]
        public async Task RegisterCommand_UserExists_Fail()
        {
            _userRepository.GetEntityByEmailAsync(Arg.Any<string>()).Returns(new User());

            var command = new RegisterCommand { Nickname = "nick", Email = "mail" };
            var res = await new RegisterHandler(_userFactory, _userRepository, _authService, _responseFactory)
                .Handle(command, default);

            Assert.False(res.Success);
            Assert.NotEmpty(res.Error);
        }
    }
}
