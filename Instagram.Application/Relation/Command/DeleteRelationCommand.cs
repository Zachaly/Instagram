using Instagram.Application.Abstraction;
using Instagram.Database.Repository;
using Instagram.Models.RelationImage.Request;
using Instagram.Models.Response;
using MediatR;
using System.Transactions;

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

        public async Task<ResponseModel> Handle(DeleteRelationCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var images = await _relationImageRepository
                    .GetAsync(new GetRelationImageRequest { RelationId = request.Id, SkipPagination = true });

                foreach(var file in images.Select(x => x.FileName)) 
                {
                    await _fileService.RemoveRelationImageAsync(file);
                }

                using(var scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
                {
                    await _relationRepository.DeleteByIdAsync(request.Id);
                    await _relationImageRepository.DeleteByRelationIdAsync(request.Id);

                    scope.Complete();
                }

                return _responseFactory.CreateSuccess();
            }
            catch (Exception ex)
            {
                return _responseFactory.CreateFailure(ex.Message);
            }
        }
    }
}
