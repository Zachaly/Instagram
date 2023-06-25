using Instagram.Application.Abstraction;
using Instagram.Database.Repository;
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

        public Task<ResponseModel> Handle(ResolvePostReportCommand request, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}
