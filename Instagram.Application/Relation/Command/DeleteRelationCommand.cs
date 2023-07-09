using Instagram.Application.Abstraction;
using Instagram.Database.Repository;
using Instagram.Models.Response;
using MediatR;

namespace Instagram.Application.Command
{
    public class DeleteRelationCommand : IRequest<ResponseModel>
    {
        public long Id { get; set; }
    }

    public class DeleteRelationHandler : IRequestHandler<DeleteRelationCommand, ResponseModel>
    {
        private readonly IRelationRepository _relationRepository;
        private readonly IRelationImageRepository _relationImageRepository;
        private readonly IFileService _fileService;
        private readonly IResponseFactory _responseFactory;

        public DeleteRelationHandler(IRelationRepository relationRepository, IRelationImageRepository relationImageRepository, 
            IFileService fileService, IResponseFactory responseFactory)
        {
            _relationRepository = relationRepository;
            _relationImageRepository = relationImageRepository;
            _fileService = fileService;
            _responseFactory = responseFactory;
        }

        public Task<ResponseModel> Handle(DeleteRelationCommand request, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}
