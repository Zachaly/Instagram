using Instagram.Application.Abstraction;
using Instagram.Application.Command;
using Instagram.Database.Repository;
using Instagram.Domain.Entity;
using Instagram.Models.Relation.Request;
using Instagram.Models.Response;
using Microsoft.AspNetCore.Http;
using NSubstitute;
using NSubstitute.ExceptionExtensions;

namespace Instagram.Tests.Unit.CommandTests
{
    public class AddRelationCommandTests
    {
        private readonly IRelationRepository _relationRepository;
        private readonly IRelationFactory _relationFactory;
        private readonly IResponseFactory _responseFactory;
        private readonly IFileService _fileService;
        private readonly IRelationImageRepository _relationImageRepository;
        private readonly AddRelationHandler _handler;

        public AddRelationCommandTests()
        {
            _relationRepository = Substitute.For<IRelationRepository>();
            _relationFactory = Substitute.For<IRelationFactory>();
            _responseFactory = ResponseFactoryMock.Create();
            _fileService = Substitute.For<IFileService>();
            _relationImageRepository = Substitute.For<IRelationImageRepository>();


            _handler = new AddRelationHandler(_relationRepository, _relationFactory, _relationImageRepository,
                _fileService, _responseFactory);
        }

        [Fact]
        public async Task Handle_Success_RelationAdded()
        {
            var relations = new List<Relation>();
            var images = new List<RelationImage>();

            _fileService.SaveRelationImagesAsync(Arg.Any<IEnumerable<IFormFile>>())
                .Returns(info => info.Arg<IEnumerable<IFormFile>>().Select(f => f.Name));

            _relationFactory.Create(Arg.Any<AddRelationRequest>())
                .Returns(info => new Relation
                    { 
                        Name = info.Arg<AddRelationRequest>().Name,
                        UserId = info.Arg<AddRelationRequest>().UserId
                    });

            _relationFactory.CreateImage(Arg.Any<long>(), Arg.Any<string>())
                .Returns(info => new RelationImage
                    {
                        FileName = info.Arg<string>(),
                        RelationId = info.Arg<long>(),
                    });

            const long NewId = 2;

            _relationRepository.InsertAsync(Arg.Any<Relation>())
                .Returns(NewId)
                .AndDoes(info => relations.Add(info.Arg<Relation>()));

            _relationImageRepository.InsertAsync(Arg.Any<RelationImage>())
                .Returns(0)
                .AndDoes(info => images.Add(info.Arg<RelationImage>()));

            var fileNames = new string[] { "file1", "file2" };

            var files = fileNames.Select(name =>
            {
                var mock = Substitute.For<IFormFile>();
                mock.Name.Returns(name);

                return mock;
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

            _relationFactory.Create(Arg.Any<AddRelationRequest>())
                .Throws(new Exception(Error));

            var res = await _handler.Handle(new AddRelationCommand(), default);

            Assert.False(res.Success);
            Assert.Equal(Error, res.Error);
        }
    }
}
