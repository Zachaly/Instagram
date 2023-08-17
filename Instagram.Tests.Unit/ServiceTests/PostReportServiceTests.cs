using Instagram.Application;
using Instagram.Application.Abstraction;
using Instagram.Database.Repository;
using Instagram.Domain.Entity;
using Instagram.Models.PostReport.Request;
using Instagram.Models.Response;
using NSubstitute;
using NSubstitute.ExceptionExtensions;

namespace Instagram.Tests.Unit.ServiceTests
{
    public class PostReportServiceTests
    {
        private readonly IPostReportRepository _postReportRepository;
        private readonly IPostReportFactory _postReportFactory;
        private readonly IResponseFactory _responseFactory;
        private readonly PostReportService _service;

        public PostReportServiceTests()
        {
            _postReportRepository = Substitute.For<IPostReportRepository>();
            _postReportFactory = Substitute.For<IPostReportFactory>();
            _responseFactory = ResponseFactoryMock.Create();

            _service = new PostReportService(_postReportRepository, _postReportFactory, _responseFactory);
        }

        [Fact]
        public async Task AddAsync_Success()
        {
            var reports = new List<PostReport>();
            const long NewId = 1;

            _postReportRepository.InsertAsync(Arg.Any<PostReport>())
                .Returns(NewId)
                .AndDoes(info => reports.Add(info.Arg<PostReport>()));

            _postReportFactory.Create(Arg.Any<AddPostReportRequest>())
                .Returns(info => new PostReport { PostId = info.Arg<AddPostReportRequest>().PostId });

            var request = new AddPostReportRequest { PostId = 2 };

            var res = await _service.AddAsync(request);

            Assert.True(res.Success);
            Assert.Contains(reports, x => x.PostId == request.PostId);
            Assert.Equal(NewId, res.NewEntityId);
        }

        [Fact]
        public async Task AddAsync_ExceptionThrown_Failure()
        {
            const string Error = "err";

            _postReportFactory.Create(Arg.Any<AddPostReportRequest>())
                .Throws(new Exception(Error));

            var request = new AddPostReportRequest { PostId = 2 };

            var res = await _service.AddAsync(request);

            Assert.False(res.Success);
            Assert.Equal(Error, res.Error);
        }
    }
}
