using Instagram.Application.Abstraction;
using Instagram.Database.Repository;
using Instagram.Models.Response;
using Instagram.Models.User;
using Instagram.Models.User.Request;
using Instagram.Models.UserClaim.Request;
using MediatR;

namespace Instagram.Application.Command
{
    public class LoginCommand : LoginRequest, IRequest<DataResponseModel<LoginResponse>>
    {
    }

    public class LoginHandler : IRequestHandler<LoginCommand, DataResponseModel<LoginResponse>>
    {
        const string FailMessage = "Invalid password or email!";
        private readonly IUserRepository _userRepository;
        private readonly IAuthService _authService;
        private readonly IUserFactory _userFactory;
        private readonly IResponseFactory _responseFactory;
        private readonly IUserClaimRepository _userClaimRepository;

        public LoginHandler(IUserRepository userRepository, IAuthService authService,
            IUserFactory userFactory, IResponseFactory responseFactory, IUserClaimRepository userClaimRepository)
        {
            _userRepository = userRepository;
            _authService = authService;
            _userFactory = userFactory;
            _responseFactory = responseFactory;
            _userClaimRepository = userClaimRepository;
        }

        public async Task<DataResponseModel<LoginResponse>> Handle(LoginCommand request, CancellationToken cancellationToken)
        {
            var user = await _userRepository.GetEntityByEmailAsync(request.Email);
            if(user is null)
            {
                return _responseFactory.CreateFailure<LoginResponse>(FailMessage);
            }

            if(!(await _authService.VerifyPasswordAsync(request.Password, user.PasswordHash)))
            {
                return _responseFactory.CreateFailure<LoginResponse>(FailMessage);
            }

            var claims = await _userClaimRepository.GetEntitiesAsync(new GetUserClaimRequest { UserId = user.Id, SkipPagination = true });

            var token = await _authService.GenerateTokenAsync(user, claims);

            return _responseFactory.CreateSuccess(_userFactory.CreateLoginResponse(user.Id, token, user.Email, claims));
        }
    }
}
