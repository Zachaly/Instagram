using Instagram.Application.Abstraction;
using Instagram.Database.Repository;
using Instagram.Models.Relation.Request;
using Instagram.Models.Response;
using MediatR;
using Microsoft.AspNetCore.Http;

namespace Instagram.Application.Command
{
    public class AddRelationCommand : AddRelationRequest, IValidatedRequest
    {
        public IEnumerable<IFormFile> Files { get; set; }
    }

    public class AddRelationHandler : IRequestHandler<AddRelationCommand, ResponseModel>
    {
        private readonly IRelationRepository _relationRepository;
        private readonly IRelationFactory _relationFactory;
        private readonly IFileService _fileService;
        private readonly IResponseFactory _responseFactory;
        private readonly IRelationImageRepository _relationImageRepository;

        public AddRelationHandler(IRelationRepository relationRepository, IRelationFactory relationFactory,
            IRelationImageRepository relationImageRepository,
            IFileService fileService, IResponseFactory responseFactory) 
        {
            _relationRepository = relationRepository;
            _relationFactory = relationFactory;
            _fileService = fileService;
            _responseFactory = responseFactory;
            _relationImageRepository = relationImageRepository;
        }

        public Task<ResponseModel> Handle(AddRelationCommand request, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}
