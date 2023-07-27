using Instagram.Application.Abstraction;
using Instagram.Database.Repository;
using Instagram.Models.Response;
using MediatR;
using Microsoft.AspNetCore.Http;

namespace Instagram.Application.Command
{
    public class AddUserStoryImagesCommand : IValidatedRequest
    {
        public long UserId { get; set; }
        public IFormFile Images { get; set; }
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

        public Task<ResponseModel> Handle(AddUserStoryImagesCommand request, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}
