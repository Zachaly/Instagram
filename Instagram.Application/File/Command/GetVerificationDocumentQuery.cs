﻿using Instagram.Application.Abstraction;
using Instagram.Database.Repository;
using MediatR;

namespace Instagram.Application.Command
{
    public class GetVerificationDocumentQuery : IRequest<FileStream>
    {
        public long Id { get; set; }
    }

    public class GetVerificationDocumentHandler : IRequestHandler<GetVerificationDocumentQuery, FileStream>
    {
        private readonly IAccountVerificationRepository _accountVerificationRepository;
        private readonly IFileService _fileService;

        public GetVerificationDocumentHandler(IAccountVerificationRepository accountVerificationRepository, IFileService fileService)
        {
            _accountVerificationRepository = accountVerificationRepository;
            _fileService = fileService;
        }

        public Task<FileStream> Handle(GetVerificationDocumentQuery request, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}
