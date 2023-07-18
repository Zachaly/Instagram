using Instagram.Application.Abstraction;
using Instagram.Database.Repository;
using Instagram.Models.Response;
using Instagram.Models.AccountVerification.Request;
using MediatR;
using Microsoft.AspNetCore.Http;

namespace Instagram.Application.Command
{
    public class AddAccountVerificationCommand : AddAccountVerificationRequest, IValidatedRequest
    {
        public IFormFile Document { get; set; }
    }

    public class AddAccountVerificationHandler : IRequestHandler<AddAccountVerificationCommand, ResponseModel>
    {
        private readonly IAccountVerificationRepository _accountVerificationRepository;
        private readonly IAccountVerificationFactory _accountVerificationFactory;
        private readonly IResponseFactory _responseFactory;
        private readonly IFileService _fileService;

        public AddAccountVerificationHandler(IAccountVerificationRepository accountVerificationRepository, IAccountVerificationFactory accountVerificationFactory,
            IResponseFactory responseFactory, IFileService fileService)
        {
            _accountVerificationRepository = accountVerificationRepository;
            _accountVerificationFactory = accountVerificationFactory;
            _responseFactory = responseFactory;
            _fileService = fileService;
        }

        public Task<ResponseModel> Handle(AddAccountVerificationCommand request, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}
