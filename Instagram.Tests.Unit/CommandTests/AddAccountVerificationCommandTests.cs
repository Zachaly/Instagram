using Instagram.Application.Abstraction;
using Instagram.Application.Command;
using Instagram.Database.Repository;
using Instagram.Domain.Entity;
using Instagram.Models.Response;
using Instagram.Models.AccountVerification.Request;
using Microsoft.AspNetCore.Http;
using Moq;

namespace Instagram.Tests.Unit.CommandTests
{
    public class AddAccountVerificationCommandTests
    {
        private readonly Mock<IAccountVerificationRepository> _accountVerificationRepository;
        private readonly Mock<IAccountVerificationFactory> _accountVerificationFactory;
        private readonly Mock<IFileService> _fileService;
        private readonly Mock<IResponseFactory> _responseFactory;
        private readonly AddAccountVerificationHandler _handler;

        public AddAccountVerificationCommandTests()
        {
            _accountVerificationRepository = new Mock<IAccountVerificationRepository>();
            _accountVerificationFactory = new Mock<IAccountVerificationFactory>();
            _fileService = new Mock<IFileService>();
            _responseFactory = new Mock<IResponseFactory>();

            _responseFactory.Setup(x => x.CreateSuccess(It.IsAny<long>()))
                .Returns((long id) => new ResponseModel { Success = true, NewEntityId = id });

            _responseFactory.Setup(x => x.CreateFailure(It.IsAny<string>()))
                .Returns((string error) => new ResponseModel { Success = false, Error = error });

            _handler = new AddAccountVerificationHandler(_accountVerificationRepository.Object, _accountVerificationFactory.Object,
                _responseFactory.Object, _fileService.Object);
        }

        [Fact]
        public async Task Handle_Success()
        {
            _accountVerificationFactory.Setup(x => x.Create(It.IsAny<AddAccountVerificationRequest>(), It.IsAny<string>()))
                .Returns((AddAccountVerificationRequest request, string fileName) => new AccountVerification
                {
                    UserId = request.UserId,
                    DocumentFileName = fileName,
                });

            _fileService.Setup(x => x.SaveVerificationDocumentAsync(It.IsAny<IFormFile>()))
                .ReturnsAsync((IFormFile file) => file.FileName);

            var verifications = new List<AccountVerification>();

            const long NewId = 2;

            _accountVerificationRepository.Setup(x => x.InsertAsync(It.IsAny<AccountVerification>()))
                .Callback((AccountVerification accountVerification) => verifications.Add(accountVerification))
                .ReturnsAsync(NewId);

            const string FileName = "document";
            var file = new Mock<IFormFile>();
            file.Setup(x => x.FileName).Returns(FileName);

            var command = new AddAccountVerificationCommand
            {
                UserId = 1,
                Document = file.Object,
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
            _fileService.Setup(x => x.SaveProfilePictureAsync(It.IsAny<IFormFile>()))
                .ThrowsAsync(new Exception(Error));

            var res = await _handler.Handle(new AddAccountVerificationCommand(), default);

            Assert.False(res.Success);
            Assert.Equal(Error, res.Error);
        }
    }
}
