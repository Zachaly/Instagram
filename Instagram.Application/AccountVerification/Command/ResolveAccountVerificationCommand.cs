using Instagram.Application.Abstraction;
using Instagram.Database.Repository;
using Instagram.Models.Response;
using Instagram.Models.User.Request;
using MediatR;
using System.Transactions;

namespace Instagram.Application.Command
{
    public class ResolveAccountVerificationCommand : IValidatedRequest
    {
        public long Id { get; set; }
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

        public async Task<ResponseModel> Handle(ResolveAccountVerificationCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var verification = await _accountVerificationRepository.GetEntityByIdAsync(request.Id);

                using(var scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
                {
                    if (request.Accepted)
                    {
                        var updateRequest = new UpdateUserRequest
                        {
                            Id = verification.UserId,
                            Verified = true
                        };

                        await _userRepository.UpdateAsync(updateRequest);
                    }

                    await _accountVerificationRepository.DeleteByIdAsync(request.Id);
                    
                    scope.Complete();
                }

                await _fileService.RemoveVerificationDocumentAsync(verification.DocumentFileName);

                return _responseFactory.CreateSuccess();
            }
            catch(Exception ex)
            {
                return _responseFactory.CreateFailure(ex.Message);
            }
        }
    }
}
