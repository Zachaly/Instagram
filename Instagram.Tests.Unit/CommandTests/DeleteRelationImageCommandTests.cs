using Instagram.Application.Abstraction;
using Instagram.Application.Command;
using Instagram.Database.Repository;
using Instagram.Domain.Entity;
using Instagram.Models.RelationImage;
using NSubstitute;
using NSubstitute.ExceptionExtensions;

namespace Instagram.Tests.Unit.CommandTests
{
    public class DeleteRelationImageCommandTests
    {
        private readonly IRelationImageRepository _relationImageRepository;
        private readonly IFileService _fileService;
        private readonly IResponseFactory _responseFactory;
        private readonly DeleteRelationImageHandler _handler;

        public DeleteRelationImageCommandTests()
        {
            _relationImageRepository = Substitute.For<IRelationImageRepository>();
            _fileService = Substitute.For<IFileService>();
            _responseFactory = ResponseFactoryMock.Create();

            _handler = new DeleteRelationImageHandler(_relationImageRepository, _fileService, _responseFactory);
        }

        [Fact]
        public async Task Handle_Success()
        {
            const long IdToDelete = 2;

            var images = new List<RelationImage>
            {
                new RelationImage { Id = 1, FileName = "fname" },
                new RelationImage { Id = IdToDelete, FileName = "fname" },
                new RelationImage { Id = 3, FileName = "fname" },
                new RelationImage { Id = 4, FileName = "fname" },
            };

            var fileRemoved = false;

            _relationImageRepository.GetByIdAsync(Arg.Any<long>())
                .Returns(info => images.Where(img => img.Id == info.Arg<long>()).Select(img => new RelationImageModel
                {
                    Id = img.Id,
                    FileName = img.FileName,
                }).FirstOrDefault());

            _relationImageRepository.DeleteByIdAsync(Arg.Any<long>())
                .Returns(Task.CompletedTask)
                .AndDoes(info => images.RemoveAll(img => img.Id == info.Arg<long>()));

            _fileService.RemoveRelationImageAsync(Arg.Any<string>()).Returns(Task.CompletedTask);

            var res = await _handler.Handle(new DeleteRelationImageCommand { Id = IdToDelete }, default);

            Assert.True(res.Success);
            Assert.Equal(1, _fileService.GetMethodCallsNumber(nameof(_fileService.RemoveRelationImageAsync)));
            Assert.DoesNotContain(images, x => x.Id == IdToDelete);
        }

        [Fact]
        public async Task Handle_ExceptionThrown_Failure()
        {
            const string Error = "Err";

            _relationImageRepository.GetByIdAsync(Arg.Any<long>()).ThrowsAsync(new Exception(Error));

            var res = await _handler.Handle(new DeleteRelationImageCommand(), default);

            Assert.False(res.Success);
            Assert.Equal(Error, res.Error);
        }
    }
}
