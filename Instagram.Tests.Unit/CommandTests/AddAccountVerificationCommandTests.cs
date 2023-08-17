using Instagram.Application.Abstraction;
using Instagram.Application.Command;
using Instagram.Database.Repository;
using Instagram.Domain.Entity;
using Instagram.Models.AccountVerification.Request;
using Microsoft.AspNetCore.Http;
using NSubstitute;
using NSubstitute.ExceptionExtensions;

namespace Instagram.Tests.Unit.CommandTests
{
    public class AddAccountVerificationCommandTests
    {
        private readonly IAccountVerificationRepository _accountVerificationRepository;
        private readonly IAccountVerificationFactory _accountVerificationFactory;
        private readonly IFileService _fileService;
        private readonly IResponseFactory _responseFactory;
        private readonly AddAccountVerificationHandler _handler;

        public AddAccountVerificationCommandTests()
        {
            _accountVerificationRepository = Substitute.For<IAccountVerificationRepository>();
            _accountVerificationFactory = Substitute.For<IAccountVerificationFactory>();
            _fileService = Substitute.For<IFileService>();
            _responseFactory = ResponseFactoryMock.Create();

            _handler = new AddAccountVerificationHandler(_accountVerificationRepository, _accountVerificationFactory,
                _responseFactory, _fileService);
        }

        [Fact]
        public async Task Handle_Success()
        {

            _accountVerificationFactory.Create(Arg.Any<AddAccountVerificationRequest>(), Arg.Any<string>())
                .Returns(info => new AccountVerification
                    {
                        UserId = info.Arg<AddAccountVerificationRequest>().UserId,
                        DocumentFileName = info.Arg<string>()
                    });

            _fileService.SaveVerificationDocumentAsync(Arg.Any<IFormFile>())
                .Returns(info => info.Arg<IFormFile>().FileName);

            var verifications = new List<AccountVerification>();

            const long NewId = 2;

            _accountVerificationRepository.InsertAsync(Arg.Any<AccountVerification>())
                .Returns(Task.FromResult(NewId))
                .AndDoes(info => verifications.Add(info.Arg<AccountVerification>()));
                

            const string FileName = "document";
            var file = Substitute.For<IFormFile>();
            file.FileName.Returns(FileName);

            var command = new AddAccountVerificationCommand
            {
                UserId = 1,
                Document = file,
            };

            var res = await _handler.Handle(command, default);

            Assert.True(res.Success);
            Assert.Contains(verifications, x => x.UserId == command.UserId && x.DocumentFileName == FileName);
            Assert.Equal(NewId, res.NewEntityId);
        }

        [Fact]
        public async Task Handle_ExceptionThrown_Failure()
        {
            const string Error = "err";
            _fileService.SaveVerificationDocumentAsync(Arg.Any<IFormFile>())
                .ThrowsAsync(new Exception(Error));

            var res = await _handler.Handle(new AddAccountVerificationCommand(), default);

            Assert.False(res.Success);
            Assert.Equal(Error, res.Error);
        }
    }
}
