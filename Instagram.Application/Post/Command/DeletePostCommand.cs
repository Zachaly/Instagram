using Instagram.Application.Abstraction;
using Instagram.Database.Repository;
using Instagram.Models.Response;
using MediatR;

namespace Instagram.Application.Command
{
    public class DeletePostCommand : IRequest<ResponseModel>
    {
        public long Id { get; set; }
    }

    public class DeletePostHandler : IRequestHandler<DeletePostCommand, ResponseModel>
    {
        private readonly IPostRepository _postRepository;
        private readonly IFileService _fileService;
        private readonly IResponseFactory _responseFactory;

        public DeletePostHandler(IPostRepository postRepository, IFileService fileService, IResponseFactory responseFactory)
        {
            _postRepository = postRepository;
            _fileService = fileService;
            _responseFactory = responseFactory;
        }

        public async Task<ResponseModel> Handle(DeletePostCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var post = await _postRepository.GetEntityByIdAsync(request.Id);

                if(post is null)
                {
                    return _responseFactory.CreateFailure("Post not found!");
                }

                

                await _postRepository.DeleteByIdAsync(request.Id);

                return _responseFactory.CreateSuccess();
            }
            catch (Exception ex)
            {
                return _responseFactory.CreateFailure(ex.Message);
            }
        }
    }
}
