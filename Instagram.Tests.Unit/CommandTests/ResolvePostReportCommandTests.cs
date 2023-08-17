using Instagram.Application.Abstraction;
using Instagram.Application.Command;
using Instagram.Database.Repository;
using Instagram.Domain.Entity;
using Instagram.Models.PostReport.Request;
using Instagram.Models.Response;
using Instagram.Models.UserBan.Request;
using Instagram.Models.UserClaim.Request;
using MediatR;
using NSubstitute;
using NSubstitute.ExceptionExtensions;

namespace Instagram.Tests.Unit.CommandTests
{
    public class ResolvePostReportCommandTests
    {
        private readonly IPostReportRepository _postReportRepository;
        private readonly IResponseFactory _responseFactory;
        private readonly IMediator _mediator;
        private readonly IUserBanService _userBanService;
        private readonly IUserClaimService _userClaimService;
        private readonly ResolvePostReportHandler _handler;

        public ResolvePostReportCommandTests()
        {
            _postReportRepository = Substitute.For<IPostReportRepository>();
            _responseFactory = ResponseFactoryMock.Create();
            _mediator = Substitute.For<IMediator>();
            _userBanService = Substitute.For<IUserBanService>();
            _userClaimService = Substitute.For<IUserClaimService>();

            _handler = new ResolvePostReportHandler(_postReportRepository, _responseFactory, _userBanService,
                _mediator, _userClaimService);
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

            _postReportRepository.UpdateByPostIdAsync(Arg.Any<UpdatePostReportRequest>(), Arg.Any<long>())
                .Returns(Task.CompletedTask)
                .AndDoes(info => 
                {
                    var request = info.Arg<UpdatePostReportRequest>();

                    foreach (var report in reports.Where(rep => rep.PostId == info.Arg<long>()))
                    {
                        report.Accepted = request.Accepted;
                        report.Resolved = request.Resolved ?? false;
                    }
                });

            _mediator.Send(Arg.Any<DeletePostCommand>(), default)
                .Returns(new ResponseModel { Success = true })
                .AndDoes(info => posts.RemoveAll(p => p.Id == info.Arg<DeletePostCommand>().Id));

            _userBanService.AddAsync(Arg.Any<AddUserBanRequest>())
                .Returns(new ResponseModel { Success = true })
                .AndDoes(info => bans.Add(new UserBan
                    {
                        UserId = info.Arg<AddUserBanRequest>().UserId,
                        EndDate = info.Arg<AddUserBanRequest>().EndDate
                    }));

            _userClaimService.AddAsync(Arg.Any<AddUserClaimRequest>())
                .Returns(new ResponseModel { Success = true })
                .AndDoes(info => claims.Add(new UserClaim
                    {
                        UserId = info.Arg<AddUserClaimRequest>().UserId,
                        Value = info.Arg<AddUserClaimRequest>().Value
                    }));

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

            _postReportRepository.UpdateByPostIdAsync(Arg.Any<UpdatePostReportRequest>(), Arg.Any<long>())
                .ThrowsAsync(new Exception(Error));

            var command = new ResolvePostReportCommand { Accepted = true, Id = 1, PostId = 2 };

            var res = await _handler.Handle(command, default);

            Assert.False(res.Success);
            Assert.Equal(Error, res.Error);
        }

        [Fact]
        public async Task Handle_ReportNotAccepted_Success()
        {
            var report = new PostReport();

            _postReportRepository.UpdateByIdAsync(Arg.Any<UpdatePostReportRequest>(), Arg.Any<long>())
                .Returns(Task.CompletedTask)
                .AndDoes(info =>
                {
                    var request = info.Arg<UpdatePostReportRequest>();
                    report.Id = info.Arg<long>();
                    report.Accepted = request.Accepted;
                    report.Resolved = request.Resolved ?? false;
                });

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

            _postReportRepository.UpdateByIdAsync(Arg.Any<UpdatePostReportRequest>(), Arg.Any<long>())
                .ThrowsAsync(new Exception(Error));

            var command = new ResolvePostReportCommand { Accepted = false, Id = 1, PostId = 2 };

            var res = await _handler.Handle(command, default);

            Assert.False(res.Success);
            Assert.Equal(Error, res.Error);
        }
    }
}
