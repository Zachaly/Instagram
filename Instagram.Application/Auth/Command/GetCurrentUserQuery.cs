using Instagram.Application.Abstraction;
using Instagram.Application.Auth.Abstraction;
using Instagram.Database.Repository;
using Instagram.Models.Response;
using Instagram.Models.User;
using MediatR;

namespace Instagram.Application.Auth.Command
{
    public class GetCurrentUserQuery : IRequest<DataResponseModel<LoginResponse>>
    {

    }

    public class GetCurrentUserQueryHandler : IRequestHandler<GetCurrentUserQuery, DataResponseModel<LoginResponse>>
    {
        private readonly IAuthService _authService;
        private readonly IUserRepository _userRepository;
        private readonly IUserClaimRepository _userClaimRepository;
        private readonly IUserDataService _userDataService;

        public GetCurrentUserQueryHandler(IAuthService authService, IUserRepository userRepository,
            IUserClaimRepository userClaimRepository, IUserDataService userDataService)
        {
            _authService = authService;
            _userRepository = userRepository;
            _userClaimRepository = userClaimRepository;
            _userDataService = userDataService;
        }

        public Task<DataResponseModel<LoginResponse>> Handle(GetCurrentUserQuery request, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}
