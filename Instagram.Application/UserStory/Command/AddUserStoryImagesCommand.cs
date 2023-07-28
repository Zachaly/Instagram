using Instagram.Application.Abstraction;
using Instagram.Database.Repository;
using Instagram.Models.Response;
using MediatR;
using Microsoft.AspNetCore.Http;
using System.Transactions;

namespace Instagram.Application.Command
{
    public class AddUserStoryImagesCommand : IValidatedRequest
    {
        public long UserId { get; set; }
        public IEnumerable<IFormFile> Images { get; set; }
    }

    public class AddUserStoryImagesHandler : IRequestHandler<AddUserStoryImagesCommand, ResponseModel>
    {
        private readonly IUserStoryImageRepository _userStoryImageRepository;
        private readonly IUserStoryFactory _userStoryFactory;
        private readonly IResponseFactory _responseFactory;
        private readonly IFileService _fileService;

        public AddUserStoryImagesHandler(IUserStoryImageRepository userStoryImageRepository, IUserStoryFactory userStoryFactory, 
            IResponseFactory responseFactory, IFileService fileService)
        {
            _userStoryImageRepository = userStoryImageRepository;
            _userStoryFactory = userStoryFactory;
            _responseFactory = responseFactory;
            _fileService = fileService;
        }

        public async Task<ResponseModel> Handle(AddUserStoryImagesCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var fileNames = await _fileService.SaveStoryImagesAsync(request.Images);

                var images = fileNames.Select(name => _userStoryFactory.Create(request.UserId, name));

                using(var scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
                {
                    foreach(var image in images)
                    {
                        await _userStoryImageRepository.InsertAsync(image);
                    }

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
