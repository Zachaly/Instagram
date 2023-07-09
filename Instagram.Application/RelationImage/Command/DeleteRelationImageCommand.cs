using Instagram.Application.Abstraction;
using Instagram.Database.Repository;
using Instagram.Models.Response;
using MediatR;

namespace Instagram.Application.Command
{
    public class DeleteRelationImageCommand : IRequest<ResponseModel>
    {
        public long Id { get; set; }
    }

    public class DeleteRelationImageHandler : IRequestHandler<DeleteRelationImageCommand, ResponseModel>
    {
        private readonly IRelationImageRepository _relationImageRepository;
        private readonly IFileService _fileService;
        private readonly IResponseFactory _responseFactory;

        public DeleteRelationImageHandler(IRelationImageRepository relationImageRepository, IFileService fileService,
            IResponseFactory responseFactory)
        {
            _relationImageRepository = relationImageRepository;
            _fileService = fileService;
            _responseFactory = responseFactory;
        }

        public Task<ResponseModel> Handle(DeleteRelationImageCommand request, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}
