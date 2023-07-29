using Instagram.Application.Abstraction;
using Instagram.Database.Repository;
using Instagram.Models.Response;
using MediatR;

namespace Instagram.Application.Command
{
    public class DeleteUserStoryImageCommand : IRequest<ResponseModel>
    {
        public long Id { get; set; }
    }

    public class DeleteUserStoryImageHandler : IRequestHandler<DeleteUserStoryImageCommand, ResponseModel>
    {
        private readonly IUserStoryImageRepository _userStoryImageRepository;
        private readonly IFileService _fileService;
        private readonly IResponseFactory _responseFactory;

        public DeleteUserStoryImageHandler(IUserStoryImageRepository userStoryImageRepository, IFileService fileService,
            IResponseFactory responseFactory)
        {
            _userStoryImageRepository = userStoryImageRepository;
            _fileService = fileService;
            _responseFactory = responseFactory;
        }

        public async Task<ResponseModel> Handle(DeleteUserStoryImageCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var image = await _userStoryImageRepository.GetEntityByIdAsync(request.Id);

                await _userStoryImageRepository.DeleteByIdAsync(request.Id);

                await _fileService.RemoveStoryImageAsync(image.FileName);

                return _responseFactory.CreateSuccess();
            }
            catch(Exception ex)
            {
                return _responseFactory.CreateFailure(ex.Message);
            }
        }
    }
}
