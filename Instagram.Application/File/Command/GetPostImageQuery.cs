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
        private readonly IPostRepository _postRepository;
        private readonly IFileService _fileService;

        public GetPostImageHandler(IPostRepository postRepository, IFileService fileService)
        {
            _postRepository = postRepository;
            _fileService = fileService;
        }
        public async Task<FileStream> Handle(GetPostImageQuery request, CancellationToken cancellationToken)
        {
            var post = await _postRepository.GetEntityByIdAsync(request.Id);

            return await _fileService.GetPostImageAsync("");
        }
    }
}
