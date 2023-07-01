using Instagram.Application.Abstraction;
using Instagram.Application.Command;
using Instagram.Database.Repository;
using Instagram.Domain.Entity;
using Instagram.Models.PostReport.Request;
using Instagram.Models.Response;
using Instagram.Models.UserBan.Request;
using Instagram.Models.UserClaim.Request;
using MediatR;
using Moq;

namespace Instagram.Tests.Unit.CommandTests
{
    public class ResolvePostReportCommandTests
    {
        private readonly Mock<IPostReportRepository> _postReportRepository;
        private readonly Mock<IResponseFactory> _responseFactory;
        private readonly Mock<IMediator> _mediator;
        private readonly Mock<IUserBanService> _userBanService;
        private readonly Mock<IUserClaimService> _userClaimService;
        private readonly ResolvePostReportHandler _handler;

        public ResolvePostReportCommandTests()
        {
            _postReportRepository = new Mock<IPostReportRepository>();
            _responseFactory = new Mock<IResponseFactory>();
            _mediator = new Mock<IMediator>();
            _userBanService = new Mock<IUserBanService>();
            _userClaimService = new Mock<IUserClaimService>();

            _handler = new ResolvePostReportHandler(_postReportRepository.Object, _responseFactory.Object,
                _userBanService.Object, _mediator.Object,
                _userClaimService.Object);
        }

        [Fact]
        public async Task Handle_ReportAccepted_Success()
        {
            const long PostId = 2;
            var reports = new List<PostReport>
            {
                new PostReport { PostId = PostId },
                new PostReport { PostId = 1 },
                new PostReport { PostId = PostId },
                new PostReport { PostId = 3 },
                new PostReport { PostId = PostId },
                new PostReport { PostId = 4 },
            };

            var posts = new List<Post>
            {
                new Post { Id = 1, },
                new Post { Id = PostId },
            };

            var bans = new List<UserBan>();

            var claims = new List<UserClaim>();

            _postReportRepository.Setup(x => x.UpdateByPostIdAsync(It.IsAny<UpdatePostReportRequest>(), It.IsAny<long>()))
                .Callback((UpdatePostReportRequest request, long id) =>
                {
                    foreach(var report in reports.Where(rep => rep.PostId == id)) 
                    {
                        report.Accepted = request.Accepted;
                        report.Resolved = request.Resolved ?? false;
                    }
                });

            _mediator.Setup(x => x.Send(It.IsAny<IRequest<ResponseModel>>(), It.IsAny<CancellationToken>()))
                .Callback((IRequest<ResponseModel> command, CancellationToken _) =>
                {
                    posts.RemoveAll(x => x.Id == (command as DeletePostCommand).Id);
                }).ReturnsAsync(new ResponseModel { Success = true });

            _userBanService.Setup(x => x.AddAsync(It.IsAny<AddUserBanRequest>()))
                .Callback((AddUserBanRequest request) =>
                {
                    bans.Add(new UserBan { UserId = request.UserId, EndDate = request.EndDate });
                });

            _userClaimService.Setup(x => x.AddAsync(It.IsAny<AddUserClaimRequest>()))
                .Callback((AddUserClaimRequest request) => claims.Add(new UserClaim { UserId = request.UserId, Value = request.Value }));

            _responseFactory.Setup(x => x.CreateSuccess())
                .Returns(new ResponseModel { Success = true });

            var command = new ResolvePostReportCommand { Accepted = true, Id = 1, PostId = PostId, BanEndDate = 2137, UserId = 3 };

            var res = await _handler.Handle(command, default);

            Assert.True(res.Success);
            Assert.Contains(bans, x => x.UserId == command.UserId);
            Assert.Contains(claims, x => x.UserId == command.UserId);
            Assert.DoesNotContain(posts, x => x.Id == command.PostId);
            Assert.All(reports.Where(x => x.PostId == command.PostId), rep => {
                Assert.Equal(command.Accepted, rep.Accepted);
                Assert.True(rep.Resolved);
            });
        }

        [Fact]
        public async Task Handle_ReportAccepted_ExceptionThrown_Failure()
        {
            const string Error = "error";
            _postReportRepository.Setup(x => x.UpdateByPostIdAsync(It.IsAny<UpdatePostReportRequest>(), It.IsAny<long>()))
                .Callback((UpdatePostReportRequest request, long id) =>
                {
                    throw new Exception(Error);
                });

            _responseFactory.Setup(x => x.CreateFailure(It.IsAny<string>()))
                .Returns((string err) => new ResponseModel { Success = false, Error = err });

            var command = new ResolvePostReportCommand { Accepted = true, Id = 1, PostId = 2 };

            var res = await _handler.Handle(command, default);

            Assert.False(res.Success);
            Assert.Equal(Error, res.Error);
        }

        [Fact]
        public async Task Handle_ReportNotAccepted_Success()
        {
            var report = new PostReport();

            _postReportRepository.Setup(x => x.UpdateByIdAsync(It.IsAny<UpdatePostReportRequest>(), It.IsAny<long>()))
                .Callback((UpdatePostReportRequest request, long id) =>
                {
                    report.Id = id;
                    report.Accepted = request.Accepted;
                    report.Resolved = request.Resolved ?? false;
                });

            _responseFactory.Setup(x => x.CreateSuccess())
                .Returns(new ResponseModel { Success = true });

            var command = new ResolvePostReportCommand { Accepted = false, Id = 1, PostId = 2 };

            var res = await _handler.Handle(command, default);

            Assert.True(res.Success);
            Assert.Equal(command.Id, report.Id);
            Assert.Equal(command.Accepted, report.Accepted);
            Assert.True(report.Resolved);
        }

        [Fact]
        public async Task Handle_ReportNotAccepted_ExceptionThrown_Failure()
        {
            const string Error = "err";

            _postReportRepository.Setup(x => x.UpdateByIdAsync(It.IsAny<UpdatePostReportRequest>(), It.IsAny<long>()))
                .Callback((UpdatePostReportRequest request, long id) =>
                {
                    throw new Exception(Error);
                });

            _responseFactory.Setup(x => x.CreateFailure(It.IsAny<string>()))
                .Returns((string err) => new ResponseModel { Success = false, Error = err });

            var command = new ResolvePostReportCommand { Accepted = false, Id = 1, PostId = 2 };

            var res = await _handler.Handle(command, default);

            Assert.False(res.Success);
            Assert.Equal(Error, res.Error);
        }
    }
}
