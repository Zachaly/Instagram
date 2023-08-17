using Instagram.Application.Abstraction;
using Instagram.Application.Command;
using Instagram.Database.Repository;
using Instagram.Domain.Entity;
using Instagram.Models.RelationImage;
using Instagram.Models.RelationImage.Request;
using Instagram.Models.Response;
using NSubstitute;
using NSubstitute.ExceptionExtensions;

namespace Instagram.Tests.Unit.CommandTests
{
    public class DeleteRelationCommandTests
    {
        private readonly IRelationRepository _relationRepository;
        private readonly IRelationImageRepository _relationImageRepository;
        private readonly IFileService _fileService;
        private readonly IResponseFactory _responseFactory;
        private readonly DeleteRelationHandler _handler;

        public DeleteRelationCommandTests()
        {
            _relationRepository = Substitute.For<IRelationRepository>();
            _relationImageRepository = Substitute.For<IRelationImageRepository>();
            _fileService = Substitute.For<IFileService>();
            _responseFactory = ResponseFactoryMock.Create();

            _handler = new DeleteRelationHandler(_relationRepository, _relationImageRepository,
                _fileService, _responseFactory);
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

            _relationImageRepository.DeleteByRelationIdAsync(Arg.Any<long>())
                .Returns(Task.CompletedTask)
                .AndDoes(info => images.RemoveAll(img => img.RelationId == info.Arg<long>()));

            _relationImageRepository.GetAsync(Arg.Any<GetRelationImageRequest>())
                .Returns(info => 
                    images.Where(img => img.RelationId == info.Arg<GetRelationImageRequest>().RelationId)
                    .Select(img => new RelationImageModel { RelationId = img.RelationId }));

            _relationRepository.DeleteByIdAsync(Arg.Any<long>())
                .Returns(Task.CompletedTask)
                .AndDoes(info => relations.RemoveAll(r => r.Id == info.Arg<long>()));

            _fileService.RemoveRelationImageAsync(Arg.Any<string>()).Returns(Task.CompletedTask);

            var res = await _handler.Handle(new DeleteRelationCommand { Id = IdToDelete }, default);

            Assert.True(res.Success);
            Assert.Equal(3, _fileService.GetMethodCallsNumber(nameof(_fileService.RemoveRelationImageAsync)));
            Assert.DoesNotContain(relations, x => x.Id == IdToDelete);
            Assert.DoesNotContain(images, x => x.RelationId == IdToDelete);
        }

        [Fact]
        public async Task Handle_ExceptionThrown_Failure()
        {
            const string Error = "err";

            _relationRepository.DeleteByIdAsync(Arg.Any<long>()).ThrowsAsync(new Exception(Error));

            var res = await _handler.Handle(new DeleteRelationCommand(), default);

            Assert.False(res.Success);
            Assert.Equal(Error, res.Error);
        }
    }
}
