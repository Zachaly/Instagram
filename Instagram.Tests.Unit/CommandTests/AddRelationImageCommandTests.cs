using Instagram.Application.Abstraction;
using Instagram.Application.Command;
using Instagram.Database.Repository;
using Instagram.Domain.Entity;
using Instagram.Models.Response;
using Microsoft.AspNetCore.Http;
using NSubstitute;
using NSubstitute.ExceptionExtensions;

namespace Instagram.Tests.Unit.CommandTests
{
    public class AddRelationImageCommandTests
    {
        private readonly IRelationImageRepository _relationImageRepository;
        private readonly IResponseFactory _responseFactory;
        private readonly IFileService _fileService;
        private readonly IRelationFactory _relationFactory;
        private readonly AddRelationImageHandler _handler;

        public AddRelationImageCommandTests()
        {
            _relationImageRepository = Substitute.For<IRelationImageRepository>();
            _responseFactory = ResponseFactoryMock.Create();
            _fileService = Substitute.For<IFileService>();
            _relationFactory = Substitute.For<IRelationFactory>();

            _handler = new AddRelationImageHandler(_relationImageRepository, _fileService,
                _relationFactory, _responseFactory);
        }

        [Fact]
        public async Task Handle_Success()
        {
            var images = new List<RelationImage>();

            _relationImageRepository.InsertAsync(Arg.Any<RelationImage>())
                .Returns(0)
                .AndDoes(info => images.Add(info.Arg<RelationImage>()));

            _fileService.SaveRelationImageAsync(Arg.Any<IFormFile>())
                .Returns(info => info.Arg<IFormFile>().Name);

            _relationFactory.CreateImage(Arg.Any<long>(), Arg.Any<string>())
                .Returns(info => new RelationImage
                    {
                        FileName = info.Arg<string>(),
                        RelationId = info.Arg<long>(),
                    });

            const string FileName = "file";
            var file = Substitute.For<IFormFile>();
            file.Name.Returns(FileName);

            var command = new AddRelationImageCommand
            {
                File = file,
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

            _fileService.SaveRelationImageAsync(Arg.Any<IFormFile>()).ThrowsAsync(new Exception(Error));

            var res = await _handler.Handle(new AddRelationImageCommand(), default);

            Assert.False(res.Success);
            Assert.Equal(Error, res.Error);
        }
    }
}
