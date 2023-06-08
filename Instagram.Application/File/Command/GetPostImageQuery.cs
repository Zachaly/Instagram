using Instagram.Application.Abstraction;
using Instagram.Database.Repository;
using MediatR;

namespace Instagram.Application.Command
{
    public class GetPostImageQuery : IRequest<FileStream>
    {
        public long Id { get; set; }
    }

    public class GetPostImageHandler : IRequestHandler<GetPostImageQuery, FileStream>
    {
        private readonly IPostImageRepository _postImageRepository;
        private readonly IFileService _fileService;

        public GetPostImageHandler(IPostImageRepository postImageRepository, IFileService fileService)
        {
            _postImageRepository = postImageRepository;
            _fileService = fileService;
        }

        public async Task<FileStream> Handle(GetPostImageQuery request, CancellationToken cancellationToken)
        {
            var image = await _postImageRepository.GetEntityByIdAsync(request.Id);

            return await _fileService.GetPostImageAsync(image.File);
        }
    }
}
