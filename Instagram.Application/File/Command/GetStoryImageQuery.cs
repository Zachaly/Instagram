using Instagram.Application.Abstraction;
using Instagram.Database.Repository;
using MediatR;

namespace Instagram.Application.Command
{
    public class GetStoryImageQuery : IRequest<FileStream>
    {
        public long Id { get; set; }
    }

    public class GetStoryImageHandler : IRequestHandler<GetStoryImageQuery, FileStream>
    {
        private readonly IUserStoryImageRepository _userStoryImageRepository;
        private readonly IFileService _fileService;

        public GetStoryImageHandler(IUserStoryImageRepository userStoryImageRepository, IFileService fileService)
        {
            _userStoryImageRepository = userStoryImageRepository;
            _fileService = fileService;
        }

        public Task<FileStream> Handle(GetStoryImageQuery request, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}
