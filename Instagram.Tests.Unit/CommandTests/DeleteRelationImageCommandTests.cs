using Instagram.Application.Abstraction;
using Instagram.Application.Command;
using Instagram.Database.Repository;
using Instagram.Domain.Entity;
using Instagram.Models.RelationImage.Request;
using Instagram.Models.RelationImage;
using Instagram.Models.Response;
using Instagram.Models.User;
using Moq;
using static System.Net.Mime.MediaTypeNames;

namespace Instagram.Tests.Unit.CommandTests
{
    public class DeleteRelationImageCommandTests
    {
        private readonly Mock<IRelationImageRepository> _relationImageRepository;
        private readonly Mock<IFileService> _fileService;
        private readonly Mock<IResponseFactory> _responseFactory;
        private readonly DeleteRelationImageHandler _handler;

        public DeleteRelationImageCommandTests()
        {
            _relationImageRepository = new Mock<IRelationImageRepository>();
            _fileService = new Mock<IFileService>();
            _responseFactory = new Mock<IResponseFactory>();

            _responseFactory.Setup(x => x.CreateSuccess())
                .Returns(() => new ResponseModel { Success = true });

            _responseFactory.Setup(x => x.CreateFailure(It.IsAny<string>()))
                .Returns((string err) => new ResponseModel { Error = err, Success = false });

            _handler = new DeleteRelationImageHandler(_relationImageRepository.Object, _fileService.Object, _responseFactory.Object);
        }

        [Fact]
        public async Task Handle_Success()
        {
            const long IdToDelete = 2;

            var images = new List<RelationImage>
            {
                new RelationImage { Id = 1 },
                new RelationImage { Id = IdToDelete },
                new RelationImage { Id = 3 },
                new RelationImage { Id = 4 },
            };

            var fileRemoved = false;

            _relationImageRepository.Setup(x => x.GetAsync(It.IsAny<GetRelationImageRequest>()))
                .ReturnsAsync((GetRelationImageRequest request)
                => images.Where(x => x.RelationId == request.RelationId)
                    .Select(x => new RelationImageModel { RelationId = x.RelationId }));

            _relationImageRepository.Setup(x => x.DeleteByIdAsync(It.IsAny<long>()))
                .Callback((long id) => images.RemoveAll(x => x.Id == id));

            _fileService.Setup(x => x.RemoveRelationImageAsync(It.IsAny<string>()))
                .Callback(() => fileRemoved = true);

            var res = await _handler.Handle(new DeleteRelationImageCommand { Id = IdToDelete }, default);

            Assert.True(res.Success);
            Assert.True(fileRemoved);
            Assert.DoesNotContain(images, x => x.Id == IdToDelete);
        }

        [Fact]
        public async Task Handle_ExceptionThrown_Failure()
        {
            const string Error = "Err";

            _relationImageRepository.Setup(x => x.GetAsync(It.IsAny<GetRelationImageRequest>()))
                .Callback(() => throw new Exception(Error));

            var res = await _handler.Handle(new DeleteRelationImageCommand(), default);

            Assert.False(res.Success);
            Assert.Equal(Error, res.Error);
        }
    }
}
