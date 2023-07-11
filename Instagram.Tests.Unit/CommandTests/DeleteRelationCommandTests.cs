using Instagram.Application.Abstraction;
using Instagram.Application.Command;
using Instagram.Database.Repository;
using Instagram.Domain.Entity;
using Instagram.Models.RelationImage;
using Instagram.Models.RelationImage.Request;
using Instagram.Models.Response;
using Moq;

namespace Instagram.Tests.Unit.CommandTests
{
    public class DeleteRelationCommandTests
    {
        private readonly Mock<IRelationRepository> _relationRepository;
        private readonly Mock<IRelationImageRepository> _relationImageRepository;
        private readonly Mock<IFileService> _fileService;
        private readonly Mock<IResponseFactory> _responseFactory;
        private readonly DeleteRelationHandler _handler;

        public DeleteRelationCommandTests()
        {
            _relationRepository = new Mock<IRelationRepository>();
            _relationImageRepository = new Mock<IRelationImageRepository>();
            _fileService = new Mock<IFileService>();
            _responseFactory = new Mock<IResponseFactory>();

            _responseFactory.Setup(x => x.CreateSuccess())
                .Returns(() => new ResponseModel { Success = true });

            _responseFactory.Setup(x => x.CreateFailure(It.IsAny<string>()))
                .Returns((string err) => new ResponseModel { Error = err, Success = false });

            _handler = new DeleteRelationHandler(_relationRepository.Object, _relationImageRepository.Object,
                _fileService.Object, _responseFactory.Object);
        }

        [Fact]
        public async Task Handle_Success()
        {
            const long IdToDelete = 1;

            var relations = new List<Relation>
            {
                new Relation { Id = IdToDelete },
                new Relation { Id = 2 },
                new Relation { Id = 3 },
                new Relation { Id = 4 },
                new Relation { Id = 5 },
                new Relation { Id = 6 },
            };

            var images = new List<RelationImage>
            {
                new RelationImage { RelationId = IdToDelete, },
                new RelationImage { RelationId = IdToDelete, },
                new RelationImage { RelationId = IdToDelete, },
                new RelationImage { RelationId = 2, },
                new RelationImage { RelationId = 2, },
                new RelationImage { RelationId = 2, },
                new RelationImage { RelationId = 3, },
            };

            var filesRemoved = false;

            _relationImageRepository.Setup(x => x.DeleteByRelationIdAsync(It.IsAny<long>()))
                .Callback((long id) => images.RemoveAll(x => x.RelationId == id));

            _relationImageRepository.Setup(x => x.GetAsync(It.IsAny<GetRelationImageRequest>()))
                .ReturnsAsync((GetRelationImageRequest request)
                => images.Where(x => x.RelationId == request.RelationId)
                    .Select(x => new RelationImageModel { RelationId = x.RelationId }));

            _relationRepository.Setup(x => x.DeleteByIdAsync(It.IsAny<long>()))
                .Callback((long id) => relations.RemoveAll(x => x.Id == id));

            _fileService.Setup(x => x.RemoveRelationImageAsync(It.IsAny<string>()))
                .Callback(() => filesRemoved = true);

            var res = await _handler.Handle(new DeleteRelationCommand { Id = IdToDelete }, default);

            Assert.True(res.Success);
            Assert.True(filesRemoved);
            Assert.DoesNotContain(relations, x => x.Id == IdToDelete);
            Assert.DoesNotContain(images, x => x.RelationId == IdToDelete);
        }

        [Fact]
        public async Task Handle_ExceptionThrown_Failure()
        {
            const string Error = "err";

            _relationRepository.Setup(x => x.DeleteByIdAsync(It.IsAny<long>()))
                .Callback(() => throw new Exception(Error));

            var res = await _handler.Handle(new DeleteRelationCommand(), default);

            Assert.False(res.Success);
            Assert.Equal(Error, res.Error);
        }
    }
}
