using Instagram.Application.Abstraction;
using Instagram.Application.Command;
using Instagram.Database.Repository;
using Instagram.Domain.Entity;
using Instagram.Models.Response;
using Microsoft.AspNetCore.Http;
using Moq;

namespace Instagram.Tests.Unit.CommandTests
{
    public class AddRelationImageCommandTests
    {
        private readonly Mock<IRelationImageRepository> _relationImageRepository;
        private readonly Mock<IResponseFactory> _responseFactory;
        private readonly Mock<IFileService> _fileService;
        private readonly Mock<IRelationFactory> _relationFactory;
        private readonly AddRelationImageHandler _handler;

        public AddRelationImageCommandTests()
        {
            _relationImageRepository = new Mock<IRelationImageRepository>();
            _responseFactory = new Mock<IResponseFactory>();
            _fileService = new Mock<IFileService>();
            _relationFactory = new Mock<IRelationFactory>();

            _responseFactory.Setup(x => x.CreateSuccess())
                .Returns(() => new ResponseModel { Success = true });

            _responseFactory.Setup(x => x.CreateFailure(It.IsAny<string>()))
                .Returns((string err) => new ResponseModel { Error = err, Success = false });

            _handler = new AddRelationImageHandler(_relationImageRepository.Object, _fileService.Object,
                _relationFactory.Object, _responseFactory.Object);
        }

        [Fact]
        public async Task Handle_Success()
        {
            var images = new List<RelationImage>();

            _relationImageRepository.Setup(x => x.InsertAsync(It.IsAny<RelationImage>()))
                .Callback((RelationImage image) => images.Add(image));

            _fileService.Setup(x => x.SaveRelationImageAsync(It.IsAny<IFormFile>()))
                .ReturnsAsync((IFormFile file) => file.Name);

            _relationFactory.Setup(x => x.CreateImage(It.IsAny<long>(), It.IsAny<string>()))
                .Returns((long id, string name) => new RelationImage { FileName = name, RelationId = id });

            const string FileName = "file";
            var file = new Mock<IFormFile>();
            file.Setup(x => x.Name)
                .Returns(FileName);

            var command = new AddRelationImageCommand
            {
                File = file.Object,
                RelationId = 1
            };

            var res = await _handler.Handle(command, default);

            Assert.True(res.Success);
            Assert.Contains(images, x => x.RelationId == command.RelationId && x.FileName == FileName);
        }

        [Fact]
        public async Task Handle_ExceptionThrown_Failure()
        {
            const string Error = "err";

            _fileService.Setup(x => x.SaveRelationImageAsync(It.IsAny<IFormFile>()))
                .Callback(() => throw new Exception(Error));

            var res = await _handler.Handle(new AddRelationImageCommand(), default);

            Assert.False(res.Success);
            Assert.Equal(Error, res.Error);
        }
    }
}
