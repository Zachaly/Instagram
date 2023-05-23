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
        private readonly IResponseFactory _responseFactory;

        public RegisterHandler(IUserFactory userFactory, IUserRepository userRepository,
            IAuthService authService, IResponseFactory responseFactory)
        {
            _userFactory = userFactory;
            _userRepository = userRepository;
            _authService = authService;
            _responseFactory = responseFactory;
        }

        public async Task<ResponseModel> Handle(RegisterCommand request, CancellationToken cancellationToken)
        {
            if((await _userRepository.GetEntityByEmailAsync(request.Email)) is not null)
            {
                return _responseFactory.CreateFailure("Email already taken!");
            }

            var passwordHash = await _authService.HashPasswordAsync(request.Password);
            var user = _userFactory.Create(request, passwordHash);

            await _userRepository.InsertAsync(user);

            return _responseFactory.CreateSuccess();
        }
    }
}
