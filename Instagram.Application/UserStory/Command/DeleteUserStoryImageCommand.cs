using Instagram.Application.Abstraction;
using Instagram.Database.Repository;
using Instagram.Models.Response;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

        public Task<ResponseModel> Handle(DeleteUserStoryImageCommand request, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}
