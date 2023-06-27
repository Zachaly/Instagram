using Instagram.Application.Abstraction;
using Instagram.Application.Command;
using Instagram.Database.Repository;
using Instagram.Domain.Entity;
using Instagram.Models.PostReport.Request;
using Instagram.Models.Response;
using Moq;

namespace Instagram.Tests.Unit.CommandTests
{
    public class ResolvePostReportCommandTests
    {
        private readonly Mock<IPostReportRepository> _postReportRepository;
        private readonly Mock<IResponseFactory> _responseFactory;
        private readonly ResolvePostReportHandler _handler;

        public ResolvePostReportCommandTests()
        {
            _postReportRepository = new Mock<IPostReportRepository>();
            _responseFactory = new Mock<IResponseFactory>();

            _handler = new ResolvePostReportHandler(_postReportRepository.Object, _responseFactory.Object);
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

            _postReportRepository.Setup(x => x.UpdateByPostIdAsync(It.IsAny<UpdatePostReportRequest>(), It.IsAny<long>()))
                .Callback((UpdatePostReportRequest request, long id) =>
                {
                    foreach(var report in reports.Where(rep => rep.PostId == id)) 
                    {
                        report.Accepted = request.Accepted;
                        report.Resolved = request.Resolved ?? false;
                    }
                });

            _responseFactory.Setup(x => x.CreateSuccess())
                .Returns(new ResponseModel { Success = true });

            var command = new ResolvePostReportCommand { Accepted = true, Id = 1, PostId = PostId };

            var res = await _handler.Handle(command, default);

            Assert.True(res.Success);
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
