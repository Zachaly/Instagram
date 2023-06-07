using Instagram.Application.Abstraction;
using Instagram.Database.Repository;
using Instagram.Models.Post.Request;
using Instagram.Models.Response;
using MediatR;
using Microsoft.AspNetCore.Http;

namespace Instagram.Application.Command
{
    public class AddPostCommand : AddPostRequest, IRequest<ResponseModel>
    {
        public IEnumerable<IFormFile> Files { get; set; }
    }

    public class AddPostHandler : IRequestHandler<AddPostCommand, ResponseModel>
    {
        private readonly IPostFactory _postFactory;
        private readonly IPostRepository _postRepository;
        private readonly IFileService _fileService;
        private readonly IResponseFactory _responseFactory;

        public AddPostHandler(IPostFactory postFactory, IPostRepository postRepository,
            IFileService fileService, IResponseFactory responseFactory)
        {
            _postFactory = postFactory;
            _postRepository = postRepository;
            _fileService = fileService;
            _responseFactory = responseFactory;
        }

        public async Task<ResponseModel> Handle(AddPostCommand request, CancellationToken cancellationToken)
        {
            try
            {
                if(request.Files is null || !request.Files.Any())
                {
                    return _responseFactory.CreateFailure("No image to upload");
                }

                var fileName = await _fileService.SavePostImagesAsync(request.Files);

                var post = _postFactory.Create(request);

                await _postRepository.InsertAsync(post);

                return _responseFactory.CreateSuccess();
            }
            catch(Exception ex)
            {
                return _responseFactory.CreateFailure(ex.Message);
            }
        }
    }
}
