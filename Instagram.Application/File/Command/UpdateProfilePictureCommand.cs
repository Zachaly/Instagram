using Instagram.Application.Abstraction;
using Instagram.Database.Repository;
using Instagram.Models.Response;
using MediatR;
using Microsoft.AspNetCore.Http;

namespace Instagram.Application.Command
{
    public class UpdateProfilePictureCommand : IRequest<ResponseModel>
    {
        public long UserId { get; set; }
        public IFormFile File { get; set; }
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

        public Task<ResponseModel> Handle(UpdateProfilePictureCommand request, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}
