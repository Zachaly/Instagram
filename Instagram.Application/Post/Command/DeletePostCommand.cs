﻿using Instagram.Application.Abstraction;
using Instagram.Database.Repository;
using Instagram.Models.PostImage.Request;
using Instagram.Models.Response;
using MediatR;
using System.Transactions;

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
        private readonly IPostImageRepository _postImageRepository;

        public DeletePostHandler(IPostRepository postRepository, IFileService fileService, IResponseFactory responseFactory,
            IPostImageRepository postImageRepository)
        {
            _postRepository = postRepository;
            _fileService = fileService;
            _responseFactory = responseFactory;
            _postImageRepository = postImageRepository;
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

                var images = await _postImageRepository.GetAsync(new GetPostImageRequest { PostId = request.Id, SkipPagination = true });
                
                using(var scope = new TransactionScope())
                {
                    await _postRepository.DeleteByIdAsync(request.Id);
                    await _postImageRepository.DeleteByPostIdAsync(request.Id);

                    foreach (var image in images)
                    {
                        await _fileService.RemovePostImageAsync(image.File);
                    }
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
