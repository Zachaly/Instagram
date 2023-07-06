using Instagram.Application.Abstraction;
using Instagram.Database.Repository;
using Instagram.Models.PostReport.Request;
using Instagram.Models.Response;
using Instagram.Models.UserBan.Request;
using Instagram.Models.UserClaim.Request;
using MediatR;
using System.Transactions;

namespace Instagram.Application.Command
{
    public class ResolvePostReportCommand : IValidatedRequest
    {
        public long Id { get; set; }
        public long PostId { get; set; }
        public bool Accepted { get; set; }
        public long? UserId { get; set; }
        public long? BanEndDate { get; set; }
    }

    public class ResolvePostReportHandler : IRequestHandler<ResolvePostReportCommand, ResponseModel>
    {
        private readonly IPostReportRepository _postReportRepository;
        private readonly IResponseFactory _responseFactory;
        private readonly IUserBanService _userBanService;
        private readonly IMediator _mediator;
        private readonly IUserClaimService _userClaimService;

        public ResolvePostReportHandler(IPostReportRepository postReportRepository, IResponseFactory responseFactory,
            IUserBanService userBanService, IMediator mediator, IUserClaimService userClaimService)
        {
            _postReportRepository = postReportRepository;
            _responseFactory = responseFactory;
            _userBanService = userBanService;
            _mediator = mediator;
            _userClaimService = userClaimService;
        }

        public async Task<ResponseModel> Handle(ResolvePostReportCommand request, CancellationToken cancellationToken)
        {
            try
            {
                if(request.Accepted)
                {
                    return await HandleAccepted(request);
                }

                return await HandleNotAccepted(request);
            }
            catch(Exception ex)
            {
                return _responseFactory.CreateFailure(ex.Message);
            }
        }

        private async Task<ResponseModel> HandleAccepted(ResolvePostReportCommand command)
        {
            using(var scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
            {
                var request = new UpdatePostReportRequest
                {
                    Accepted = true,
                    ResolveTime = DateTimeOffset.Now.ToUnixTimeMilliseconds(),
                    Resolved = true
                };

                await _postReportRepository.UpdateByPostIdAsync(request, command.PostId);

                await _userBanService.AddAsync(new AddUserBanRequest { UserId = command.UserId!.Value, EndDate = command.BanEndDate!.Value });

                await _mediator.Send(new DeletePostCommand { Id = command.PostId });

                await _userClaimService.AddAsync(new AddUserClaimRequest { UserId = command.UserId.Value, Value = "Ban" }); 

                scope.Complete();
            }
            
            return _responseFactory.CreateSuccess();
        }

        private async Task<ResponseModel> HandleNotAccepted(ResolvePostReportCommand command)
        {
            var request = new UpdatePostReportRequest
            {
                Accepted = false,
                Resolved = true,
                ResolveTime = DateTimeOffset.Now.ToUnixTimeMilliseconds(),
            };

            await _postReportRepository.UpdateByIdAsync(request, command.Id);

            return _responseFactory.CreateSuccess();
        }
    }
}
