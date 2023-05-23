using Instagram.Application.Abstraction;
using Instagram.Database.Repository;
using Instagram.Models.Response;
using Instagram.Models.User.Request;
using MediatR;

namespace Instagram.Application.Command
{
    public class RegisterCommand : RegisterRequest, IRequest<ResponseModel>
    {
        
    }

    public class RegisterHandler : IRequestHandler<RegisterCommand, ResponseModel>
    {
        private readonly IUserFactory _userFactory;
        private readonly IUserRepository _userRepository;
        private readonly IAuthService _authService;

        public RegisterHandler(IUserFactory userFactory, IUserRepository userRepository, IAuthService authService)
        {
            _userFactory = userFactory;
            _userRepository = userRepository;
            _authService = authService;
        }

        public Task<ResponseModel> Handle(RegisterCommand request, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}
