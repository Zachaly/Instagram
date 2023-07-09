using Instagram.Application.Abstraction;
using Instagram.Database.Repository;
using MediatR;

namespace Instagram.Application.Command
{
    public class GetRelationImageQuery : IRequest<FileStream>
    {
        public long Id { get; set; }
    }

    public class GetRelationImageHandler : IRequestHandler<GetRelationImageQuery, FileStream>
    {
        private readonly IRelationImageRepository _relationImageRepository;
        private readonly IFileService _fileService;

        public GetRelationImageHandler(IRelationImageRepository relationImageRepository, IFileService fileService)
        {
            _relationImageRepository = relationImageRepository;
            _fileService = fileService;
        }

        public Task<FileStream> Handle(GetRelationImageQuery request, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}
