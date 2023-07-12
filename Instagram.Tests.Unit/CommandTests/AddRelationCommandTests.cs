using Instagram.Application.Abstraction;
using Instagram.Application.Command;
using Instagram.Database.Repository;
using Instagram.Domain.Entity;
using Instagram.Models.Relation.Request;
using Instagram.Models.Response;
using Instagram.Models.User;
using Microsoft.AspNetCore.Http;
using Moq;

namespace Instagram.Tests.Unit.CommandTests
{
    public class AddRelationCommandTests
    {
        private readonly Mock<IRelationRepository> _relationRepository;
        private readonly Mock<IRelationFactory> _relationFactory;
        private readonly Mock<IResponseFactory> _responseFactory;
        private readonly Mock<IFileService> _fileService;
        private readonly Mock<IRelationImageRepository> _relationImageRepository;
        private readonly AddRelationHandler _handler;

        public AddRelationCommandTests()
        {
            _relationRepository = new Mock<IRelationRepository>();
            _relationFactory = new Mock<IRelationFactory>();
            _responseFactory = new Mock<IResponseFactory>();
            _fileService = new Mock<IFileService>();
            _relationImageRepository = new Mock<IRelationImageRepository>();

            _responseFactory.Setup(x => x.CreateSuccess())
                .Returns(() => new ResponseModel { Success = true });

            _responseFactory.Setup(x => x.CreateFailure(It.IsAny<string>()))
                .Returns((string err) => new ResponseModel { Error = err, Success = false });

            _handler = new AddRelationHandler(_relationRepository.Object, _relationFactory.Object, _relationImageRepository.Object,
                _fileService.Object, _responseFactory.Object);
        }

        [Fact]
        public async Task Handle_Success_RelationAdded()
        {
            var relations = new List<Relation>();
            var images = new List<RelationImage>();

            _fileService.Setup(x => x.SaveRelationImagesAsync(It.IsAny<IEnumerable<IFormFile>>()))
                .ReturnsAsync((IEnumerable<IFormFile> files) => files.Select(x => x.Name));

            _relationFactory.Setup(x => x.Create(It.IsAny<AddRelationRequest>()))
                .Returns((AddRelationRequest request) => new Relation { Name = request.Name, UserId = request.UserId });

            _relationFactory.Setup(x => x.CreateImage(It.IsAny<long>(), It.IsAny<string>()))
                .Returns((long id, string name) => new RelationImage { FileName = name, RelationId = id });

            const long NewId = 2;

            _relationRepository.Setup(x => x.InsertAsync(It.IsAny<Relation>()))
                .Callback((Relation relation) => relations.Add(relation))
                .ReturnsAsync(NewId);

            _relationImageRepository.Setup(x => x.InsertAsync(It.IsAny<RelationImage>()))
                .Callback((RelationImage image) => images.Add(image));

            var fileNames = new string[] { "file1", "file2" };

            var files = fileNames.Select(name =>
            {
                var mock = new Mock<IFormFile>();
                mock.Setup(x => x.Name).Returns(name);

                return mock.Object;
            });

            var command = new AddRelationCommand { Files = files, Name = "name", UserId = 1 };

            var res = await _handler.Handle(command, default);

            Assert.True(res.Success);
            Assert.Contains(relations, x => x.Name == command.Name && x.UserId == command.UserId);
            Assert.Equivalent(images.Select(x => x.FileName), command.Files.Select(x => x.Name));
            Assert.All(images, image =>
            {
                Assert.Equal(NewId, image.RelationId);
            });
        }

        [Fact]
        public async Task Handle_ExceptionThrown_Failure()
        {
            const string Error = "Err";

            _relationFactory.Setup(x => x.Create(It.IsAny<AddRelationRequest>()))
                .Callback(() => throw new Exception(Error));

            var res = await _handler.Handle(new AddRelationCommand(), default);

            Assert.False(res.Success);
            Assert.Equal(Error, res.Error);
        }
    }
}
