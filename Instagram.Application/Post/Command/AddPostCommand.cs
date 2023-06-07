using Instagram.Application.Abstraction;
using Instagram.Database.Repository;
using Instagram.Models.Post.Request;
using Instagram.Models.Response;
using MediatR;
using Microsoft.AspNetCore.Http;
using System.Transactions;

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
        private readonly IPostImageRepository _postImageRepository;

        public AddPostHandler(IPostFactory postFactory, IPostRepository postRepository,
            IFileService fileService, IResponseFactory responseFactory,
            IPostImageRepository postImageRepository)
        {
            _postFactory = postFactory;
            _postRepository = postRepository;
            _fileService = fileService;
            _responseFactory = responseFactory;
            _postImageRepository = postImageRepository;
        }

        public async Task<ResponseModel> Handle(AddPostCommand request, CancellationToken cancellationToken)
        {
            try
            {
                if(request.Files is null || !request.Files.Any())
                {
                    return _responseFactory.CreateFailure("No image to upload");
                }

                var fileNames = await _fileService.SavePostImagesAsync(request.Files);

                var post = _postFactory.Create(request);

                using(var scope = new TransactionScope())
                {
                    var postId = await _postRepository.InsertAsync(post);
                    var images = _postFactory.CreateImages(fileNames, postId);

                    foreach(var image in images)
                    {
                        await _postImageRepository.InsertAsync(image);
                    }
                }

                return _responseFactory.CreateSuccess();
            }
            catch(Exception ex)
            {
                return _responseFactory.CreateFailure(ex.Message);
            }
        }
    }
}
