using Instagram.Application.Abstraction;
using Instagram.Database.Repository;
using MediatR;

namespace Instagram.Application.Command
{
    public class GetProfilePictureQuery : IRequest<FileStream>
    {
        public long UserId { get; set; }
    }

    public class GetProfilePictureHandler : IRequestHandler<GetProfilePictureQuery, FileStream>
    {
        private readonly IFileService _fileService;
        private readonly IUserRepository _userRepository;

        public GetProfilePictureHandler(IFileService fileService, IUserRepository userRepository)
        {
            _fileService = fileService;
            _userRepository = userRepository;
        }

        public Task<FileStream> Handle(GetProfilePictureQuery request, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}
