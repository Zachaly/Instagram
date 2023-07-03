using Instagram.Application.Abstraction;
using Instagram.Application.Auth.Abstraction;
using Instagram.Application.Auth.Command;
using Instagram.Database.Repository;
using Instagram.Domain.Entity;
using Instagram.Models.Response;
using Instagram.Models.User;
using Instagram.Models.UserClaim.Request;
using Moq;

namespace Instagram.Tests.Unit.CommandTests
{
    public class GetCurrentUserQueryTests
    {
        private readonly Mock<IAuthService> _authService;
        private readonly Mock<IUserRepository> _userRepository;
        private readonly Mock<IUserClaimRepository> _userClaimRepository;
        private readonly Mock<IResponseFactory> _responseFactory;
        private readonly Mock<IUserDataService> _userDataService;
        private readonly Mock<IUserFactory> _userFactory;
        private readonly GetCurrentUserHandler _handler;

        public GetCurrentUserQueryTests()
        {
            _authService = new Mock<IAuthService>();
            _userRepository = new Mock<IUserRepository>();
            _userClaimRepository = new Mock<IUserClaimRepository>();
            _responseFactory = new Mock<IResponseFactory>();
            _userDataService = new Mock<IUserDataService>();
            _userFactory = new Mock<IUserFactory>();

            _responseFactory.Setup(x => x.CreateSuccess<LoginResponse>(It.IsAny<LoginResponse>()))
                .Returns((LoginResponse response) => new DataResponseModel<LoginResponse> { Success = true, Data = response });

            _responseFactory.Setup(x => x.CreateFailure<LoginResponse>(It.IsAny<string>()))
                .Returns((string err) => new DataResponseModel<LoginResponse> { Data = null, Error = err, Success = false });

            _handler = new GetCurrentUserHandler(_authService.Object, _userRepository.Object, _userClaimRepository.Object, _userDataService.Object,
                _responseFactory.Object, _userFactory.Object);
        }

        [Fact]
        public async Task Handle_Success()
        {
            var user = new User { Id = 1 };

            _userDataService.Setup(x => x.GetCurrentUserId())
                .ReturnsAsync(user.Id);

            _userRepository.Setup(x => x.GetEntityByIdAsync(It.IsAny<long>()))
                .ReturnsAsync(user);

            _userClaimRepository.Setup(x => x.GetEntitiesAsync(It.IsAny<GetUserClaimRequest>()))
                .ReturnsAsync(Enumerable.Empty<UserClaim>());

            const string Token = "token";

            _authService.Setup(x => x.GenerateTokenAsync(It.IsAny<User>(), It.IsAny<IEnumerable<UserClaim>>()))
                .ReturnsAsync(Token);

            _userFactory.Setup(x => x.CreateLoginResponse(It.IsAny<long>(), It.IsAny<string>(),
                It.IsAny<string>(), It.IsAny<IEnumerable<UserClaim>>()))
                .Returns((long id, string token, string _, IEnumerable<UserClaim> __) => new LoginResponse
                {
                    AuthToken = token,
                    UserId = id,
                });

            var res = await _handler.Handle(new GetCurrentUserQuery(), default);

            Assert.True(res.Success);
            Assert.Equal(user.Id, res.Data.UserId);
            Assert.Equal(Token, res.Data.AuthToken);
        }

        [Fact]
        public async Task Handle_UserIdNull_Failure()
        {
            _userDataService.Setup(x => x.GetCurrentUserId())
                .ReturnsAsync(() => null);

            var res = await _handler.Handle(new GetCurrentUserQuery(), default);

            Assert.False(res.Success);
            Assert.NotEmpty(res.Error);
        }

        [Fact]
        public async Task Handle_UserNotFound_Failure()
        {
            _userDataService.Setup(x => x.GetCurrentUserId())
                .ReturnsAsync(2137);

            _userRepository.Setup(x => x.GetEntityByIdAsync(It.IsAny<long>()))
                .ReturnsAsync(() => null);

            var res = await _handler.Handle(new GetCurrentUserQuery(), default);

            Assert.False(res.Success);
            Assert.NotEmpty(res.Error);
        }

        [Fact]
        public async Task Handle_ExceptionThrown_Failure()
        {
            _userDataService.Setup(x => x.GetCurrentUserId())
                .ReturnsAsync(2137);

            _userRepository.Setup(x => x.GetEntityByIdAsync(It.IsAny<long>()))
                .ReturnsAsync(() => new User());

            const string Error = "err";

            _userClaimRepository.Setup(x => x.GetEntitiesAsync(It.IsAny<GetUserClaimRequest>()))
                .Callback(() => throw new Exception(Error));

            var res = await _handler.Handle(new GetCurrentUserQuery(), default);

            Assert.False(res.Success);
            Assert.Equal(Error, res.Error);
        }
    }
}
