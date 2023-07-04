using Instagram.Application.Abstraction;
using Instagram.Database.Repository;
using Instagram.Models.Response;
using Instagram.Models.User.Request;
using MediatR;

namespace Instagram.Application.Command
{
    public class ChangePasswordCommand : IRequest<ResponseModel>
    {
        public long UserId { get; set; }
        public string OldPassword { get; set; }
        public string NewPassword { get; set; }
    }

    public class ChangePasswordHandler : IRequestHandler<ChangePasswordCommand, ResponseModel>
    {
        private readonly IAuthService _authService;
        private readonly IUserRepository _userRepository;
        private readonly IResponseFactory _responseFactory;

        public ChangePasswordHandler(IAuthService authService, IUserRepository userRepository, IResponseFactory responseFactory)
        {
            _authService = authService;
            _userRepository = userRepository;
            _responseFactory = responseFactory;
        }

        public async Task<ResponseModel> Handle(ChangePasswordCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var user = await _userRepository.GetEntityByIdAsync(request.UserId);

                if(user is null)
                {
                    return _responseFactory.CreateFailure("User not found");
                }

                if(!await _authService.VerifyPasswordAsync(request.OldPassword, user.PasswordHash))
                {
                    return _responseFactory.CreateFailure("Invalid old password!");
                }

                var updateRequest = new UpdateUserRequest
                {
                    Id = request.UserId,
                    PasswordHash = await _authService.HashPasswordAsync(request.NewPassword)
                };

                await _userRepository.UpdateAsync(updateRequest);

                return _responseFactory.CreateSuccess();
            }
            catch (Exception ex)
            {
                return _responseFactory.CreateFailure(ex.Message);
            }
        }
    }
}
