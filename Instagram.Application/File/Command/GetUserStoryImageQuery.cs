using Instagram.Application.Abstraction;
using Instagram.Database.Repository;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Instagram.Application.Command
{
    public class GetUserStoryImageQuery : IRequest<FileStream>
    {
        public long Id { get; set; }
    }

    public class GetUserStoryImageHandler : IRequestHandler<GetUserStoryImageQuery, FileStream>
    {
        private readonly IUserStoryImageRepository _userStoryImageRepository;
        private readonly IFileService _fileService;

        public GetUserStoryImageHandler(IUserStoryImageRepository userStoryImageRepository, IFileService fileService)
        {
            _userStoryImageRepository = userStoryImageRepository;
            _fileService = fileService;
        }

        public async Task<FileStream> Handle(GetUserStoryImageQuery request, CancellationToken cancellationToken)
        {
            var image = await _userStoryImageRepository.GetEntityByIdAsync(request.Id);

            return await _fileService.GetStoryImageAsync(image.FileName);
        }
    }
}
