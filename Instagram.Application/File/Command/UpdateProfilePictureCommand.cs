using Instagram.Application.Abstraction;
using Instagram.Database.Repository;
using Instagram.Models.Response;
using Instagram.Models.User.Request;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.IdentityModel.Tokens;

namespace Instagram.Application.Command
{
    public class UpdateProfilePictureCommand : IRequest<ResponseModel>
    {
        public long UserId { get; set; }
        public IFormFile? File { get; set; }
    }

    public class UpdateProfilePictureHandler : IRequestHandler<UpdateProfilePictureCommand, ResponseModel>
    {
        private readonly IFileService _fileService;
        private readonly IUserRepository _userRepository;
        private readonly IResponseFactory _responseFactory;

        public UpdateProfilePictureHandler(IFileService fileService, IUserRepository userRepository, IResponseFactory responseFactory)
        {
            _fileService = fileService;
            _userRepository = userRepository;
            _responseFactory = responseFactory;
        }

        public async Task<ResponseModel> Handle(UpdateProfilePictureCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var user = await _userRepository.GetEntityByIdAsync(request.UserId);
                if(user is null)
                {
                    return _responseFactory.CreateFailure("User does not exist");
                }

                if(user.ProfilePicture is not null)
                {
                    await _fileService.RemoveProfilePictureAsync(user.ProfilePicture);
                }

                var fileName = await _fileService.SaveProfilePictureAsync(request.File);

                var updateRequest = new UpdateUserRequest
                {
                    Id = request.UserId,
                    ProfilePicture = fileName,
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
