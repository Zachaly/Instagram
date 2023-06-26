using Instagram.Application.Abstraction;
using Instagram.Database.Repository;
using Instagram.Models.PostReport.Request;
using Instagram.Models.Response;
using MediatR;

namespace Instagram.Application.Command
{
    public class ResolvePostReportCommand : IRequest<ResponseModel>
    {
        public long Id { get; set; }
        public long PostId { get; set; }
        public bool Accepted { get; set; }
    }

    public class ResolvePostReportHandler : IRequestHandler<ResolvePostReportCommand, ResponseModel>
    {
        private readonly IPostReportRepository _postReportRepository;
        private readonly IResponseFactory _responseFactory;

        public ResolvePostReportHandler(IPostReportRepository postReportRepository, IResponseFactory responseFactory)
        {
            _postReportRepository = postReportRepository;
            _responseFactory = responseFactory;
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
            var request = new UpdatePostReportRequest
            {
                Accepted = true,
                ResolveTime = DateTimeOffset.UtcNow.ToUnixTimeSeconds(),
                Resolved = true
            };

            await _postReportRepository.UpdateByPostIdAsync(request, command.PostId);

            return _responseFactory.CreateSuccess();
        }

        private async Task<ResponseModel> HandleNotAccepted(ResolvePostReportCommand command)
        {
            var request = new UpdatePostReportRequest
            {
                Accepted = false,
                Resolved = true,
                ResolveTime = DateTimeOffset.UtcNow.ToUnixTimeSeconds(),
            };

            await _postReportRepository.UpdateByIdAsync(request, command.Id);

            return _responseFactory.CreateSuccess();
        }
    }
}
