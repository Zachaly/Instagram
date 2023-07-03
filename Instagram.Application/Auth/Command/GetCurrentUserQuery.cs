using Instagram.Application.Abstraction;
using Instagram.Application.Auth.Abstraction;
using Instagram.Database.Repository;
using Instagram.Models.Response;
using Instagram.Models.User;
using Instagram.Models.UserClaim.Request;
using MediatR;

namespace Instagram.Application.Auth.Command
{
    public class GetCurrentUserQuery : IRequest<DataResponseModel<LoginResponse>>
    {

    }

    public class GetCurrentUserHandler : IRequestHandler<GetCurrentUserQuery, DataResponseModel<LoginResponse>>
    {
        private readonly IAuthService _authService;
        private readonly IUserRepository _userRepository;
        private readonly IUserClaimRepository _userClaimRepository;
        private readonly IUserDataService _userDataService;
        private readonly IResponseFactory _responseFactory;
        private readonly IUserFactory _userFactory;

        const string FailMessage = "User not found!";

        public GetCurrentUserHandler(IAuthService authService, IUserRepository userRepository,
            IUserClaimRepository userClaimRepository, IUserDataService userDataService,
            IResponseFactory responseFactory, IUserFactory userFactory)
        {
            _authService = authService;
            _userRepository = userRepository;
            _userClaimRepository = userClaimRepository;
            _userDataService = userDataService;
            _responseFactory = responseFactory;
            _userFactory = userFactory;
        }

        public async Task<DataResponseModel<LoginResponse>> Handle(GetCurrentUserQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var id = await _userDataService.GetCurrentUserId();

                if(!id.HasValue)
                {
                    return _responseFactory.CreateFailure<LoginResponse>(FailMessage);
                }

                var user = await _userRepository.GetEntityByIdAsync(id.Value);

                if(user is null)
                {
                    return _responseFactory.CreateFailure<LoginResponse>(FailMessage);
                }

                var claims = await _userClaimRepository.GetEntitiesAsync(new GetUserClaimRequest { UserId = id });

                var token = await _authService.GenerateTokenAsync(user, claims);

                var response = _userFactory.CreateLoginResponse(id.Value, token, user.Email, claims);

                return _responseFactory.CreateSuccess(response);
            }
            catch(Exception ex)
            {
                return _responseFactory.CreateFailure<LoginResponse>(ex.Message);
            }
        }
    }
}
