using Instagram.Application.Abstraction;
using Instagram.Database.Repository;
using Instagram.Models.Response;
using MediatR;
using Microsoft.AspNetCore.Http;

namespace Instagram.Application.Command
{
    public class AddRelationImageCommand : IValidatedRequest
    {
        public long RelationId { get; set; }
        public IFormFile File { get; set; }
    }

    public class AddRelationImageHandler : IRequestHandler<AddRelationImageCommand, ResponseModel>
    {
        private readonly IRelationImageRepository _relationImageRepository;
        private readonly IFileService _fileService;
        private readonly IResponseFactory _responseFactory;

        public AddRelationImageHandler(IRelationImageRepository relationImageRepository, IFileService fileService,
            IResponseFactory responseFactory)
        {
            _relationImageRepository = relationImageRepository;
            _fileService = fileService;
            _responseFactory = responseFactory;
        }

        public Task<ResponseModel> Handle(AddRelationImageCommand request, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}
