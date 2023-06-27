using Instagram.Application;
using Instagram.Application.Abstraction;
using Instagram.Database.Repository;
using Instagram.Domain.Entity;
using Instagram.Models.PostReport.Request;
using Instagram.Models.Response;
using Moq;

namespace Instagram.Tests.Unit.ServiceTests
{
    public class PostReportServiceTests
    {
        private readonly Mock<IPostReportRepository> _postReportRepository;
        private readonly Mock<IPostReportFactory> _postReportFactory;
        private readonly Mock<IResponseFactory> _responseFactory;
        private readonly PostReportService _service;

        public PostReportServiceTests()
        {
            _postReportRepository = new Mock<IPostReportRepository>();
            _postReportFactory = new Mock<IPostReportFactory>();
            _responseFactory = new Mock<IResponseFactory>();

            _service = new PostReportService(_postReportRepository.Object, _postReportFactory.Object, _responseFactory.Object);
        }

        [Fact]
        public async Task AddAsync_Success()
        {
            var reports = new List<PostReport>();
            const long NewId = 1;

            _postReportRepository.Setup(x => x.InsertAsync(It.IsAny<PostReport>()))
                .Callback((PostReport postReport) => reports.Add(postReport))
                .ReturnsAsync(NewId);

            _postReportFactory.Setup(x => x.Create(It.IsAny<AddPostReportRequest>()))
                .Returns((AddPostReportRequest request) => new PostReport { PostId = request.PostId });

            _responseFactory.Setup(x => x.CreateSuccess(It.IsAny<long>()))
                .Returns((long id) => new ResponseModel { Success = true, NewEntityId = id });

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

            _postReportRepository.Setup(x => x.InsertAsync(It.IsAny<PostReport>()))
                .Callback(() => throw new Exception(Error));

            _postReportFactory.Setup(x => x.Create(It.IsAny<AddPostReportRequest>()))
                .Returns((AddPostReportRequest request) => new PostReport { PostId = request.PostId });

            _responseFactory.Setup(x => x.CreateFailure(It.IsAny<string>()))
                .Returns((string err) => new ResponseModel { Success = false, Error = err });

            var request = new AddPostReportRequest { PostId = 2 };

            var res = await _service.AddAsync(request);

            Assert.False(res.Success);
            Assert.Equal(Error, res.Error);
        }
    }
}
