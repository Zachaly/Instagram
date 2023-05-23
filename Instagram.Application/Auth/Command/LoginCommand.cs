using Instagram.Application.Abstraction;
using Instagram.Database.Repository;
using Instagram.Models.Response;
using Instagram.Models.User;
using Instagram.Models.User.Request;
using MediatR;

namespace Instagram.Application.Command
{
    public class LoginCommand : LoginRequest, IRequest<DataResponseModel<LoginResponse>>
    {
    }

    public class LoginHandler : IRequestHandler<LoginCommand, DataResponseModel<LoginResponse>>
    {
        private readonly IUserRepository _userRepository;
        private readonly IAuthService _authService;
        private readonly IUserFactory _userFactory;
        private readonly IResponseFactory _responseFactory;

        public LoginHandler(IUserRepository userRepository, IAuthService authService,
            IUserFactory userFactory, IResponseFactory responseFactory)
        {
            _userRepository = userRepository;
            _authService = authService;
            _userFactory = userFactory;
            _responseFactory = responseFactory;
        }

        public Task<DataResponseModel<LoginResponse>> Handle(LoginCommand request, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}
