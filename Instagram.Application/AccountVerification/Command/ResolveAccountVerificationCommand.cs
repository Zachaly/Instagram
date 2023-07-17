using Instagram.Application.Abstraction;
using Instagram.Database.Repository;
using Instagram.Models.Response;
using MediatR;

namespace Instagram.Application.Command
{
    public class ResolveAccountVerificationCommand : IValidatedRequest
    {
        public long Id { get; set; }
        public long UserId { get; set; }
        public bool Accepted { get; set; }
    }

    public class ResolveAccountVerificationHandler : IRequestHandler<ResolveAccountVerificationCommand, ResponseModel>
    {
        private readonly IAccountVerificationRepository _accountVerificationRepository;
        private readonly IUserRepository _userRepository;
        private readonly IResponseFactory _responseFactory;
        private readonly IFileService _fileService;

        public ResolveAccountVerificationHandler(IAccountVerificationRepository accountVerificationRepository, IUserRepository userRepository,
            IResponseFactory responseFactory, IFileService fileService)
        {
            _accountVerificationRepository = accountVerificationRepository;
            _userRepository = userRepository;
            _responseFactory = responseFactory;
            _fileService = fileService;
        }

        public Task<ResponseModel> Handle(ResolveAccountVerificationCommand request, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}
