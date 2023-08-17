using Instagram.Application.Abstraction;
using Instagram.Application.Auth.Abstraction;
using Instagram.Application.Auth.Command;
using Instagram.Database.Repository;
using Instagram.Domain.Entity;
using Instagram.Models.Response;
using Instagram.Models.User;
using Instagram.Models.UserClaim.Request;
using NSubstitute;
using NSubstitute.ExceptionExtensions;
using NSubstitute.ReturnsExtensions;

namespace Instagram.Tests.Unit.CommandTests
{
    public class GetCurrentUserQueryTests
    {
        private readonly IAuthService _authService;
        private readonly IUserRepository _userRepository;
        private readonly IUserClaimRepository _userClaimRepository;
        private readonly IResponseFactory _responseFactory;
        private readonly IUserDataService _userDataService;
        private readonly IUserFactory _userFactory;
        private readonly GetCurrentUserHandler _handler;

        public GetCurrentUserQueryTests()
        {
            _authService = Substitute.For<IAuthService>();
            _userRepository = Substitute.For<IUserRepository>();
            _userClaimRepository = Substitute.For<IUserClaimRepository>();
            _responseFactory = ResponseFactoryMock.Create();
            _userDataService = Substitute.For<IUserDataService>();
            _userFactory = Substitute.For<IUserFactory>();

            _responseFactory.CreateFailure<LoginResponse>(Arg.Any<string>())
                .Returns(info => new DataResponseModel<LoginResponse>
                {
                    Error = info.Arg<string>(),
                    Success = false,
                    Data = null
                });

            _responseFactory.CreateSuccess(Arg.Any<LoginResponse>())
                .Returns(info => new DataResponseModel<LoginResponse> { Data = info.Arg<LoginResponse>(), Success = true });

            _handler = new GetCurrentUserHandler(_authService, _userRepository, _userClaimRepository, _userDataService,
                _responseFactory, _userFactory);
        }

        [Fact]
        public async Task Handle_Success()
        {
            var user = new User { Id = 1 };

            _userDataService.GetCurrentUserId().Returns(user.Id);

            _userRepository.GetEntityByIdAsync(Arg.Any<long>()).Returns(user);

            _userClaimRepository.GetEntitiesAsync(Arg.Any<GetUserClaimRequest>()).Returns(Enumerable.Empty<UserClaim>());

            const string Token = "token";

            _authService.GenerateTokenAsync(Arg.Any<User>(), Arg.Any<IEnumerable<UserClaim>>()).Returns(Token);

            _userFactory.CreateLoginResponse(Arg.Any<long>(), Arg.Any<string>(), Arg.Any<string>(), Arg.Any<IEnumerable<UserClaim>>())
                .Returns(info => new LoginResponse
                {
                    AuthToken = info.ArgAt<string>(1),
                    UserId = info.Arg<long>()
                });

            var res = await _handler.Handle(new GetCurrentUserQuery(), default);

            Assert.True(res.Success);
            Assert.Equal(user.Id, res.Data.UserId);
            Assert.Equal(Token, res.Data.AuthToken);
        }

        [Fact]
        public async Task Handle_UserIdNull_Failure()
        {
            _userDataService.GetCurrentUserId().ReturnsNull();

            var res = await _handler.Handle(new GetCurrentUserQuery(), default);

            Assert.False(res.Success);
            Assert.NotEmpty(res.Error);
        }

        [Fact]
        public async Task Handle_UserNotFound_Failure()
        {
            _userDataService.GetCurrentUserId().Returns(2137);

            _userRepository.GetEntityByIdAsync(Arg.Any<long>()).ReturnsNull();

            var res = await _handler.Handle(new GetCurrentUserQuery(), default);

            Assert.False(res.Success);
            Assert.NotEmpty(res.Error);
        }

        [Fact]
        public async Task Handle_ExceptionThrown_Failure()
        {
            const string Error = "err";
            _userDataService.GetCurrentUserId().ThrowsAsync(new Exception(Error));

            var res = await _handler.Handle(new GetCurrentUserQuery(), default);

            Assert.False(res.Success);
            Assert.Equal(Error, res.Error);
        }
    }
}
