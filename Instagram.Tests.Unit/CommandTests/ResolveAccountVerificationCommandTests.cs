using Instagram.Application.Abstraction;
using Instagram.Application.Command;
using Instagram.Database.Repository;
using Instagram.Domain.Entity;
using Instagram.Models.Response;
using Instagram.Models.User.Request;
using Moq;

namespace Instagram.Tests.Unit.CommandTests
{
    public class ResolveAccountVerificationCommandTests
    {
        private readonly Mock<IFileService> _fileService;
        private readonly Mock<IAccountVerificationRepository> _accountVerificationRepository;
        private readonly Mock<IUserRepository> _userRepository;
        private readonly Mock<IResponseFactory> _responseFactory;
        private readonly ResolveAccountVerificationHandler _handler;

        public ResolveAccountVerificationCommandTests()
        {
            _fileService = new Mock<IFileService>();
            _accountVerificationRepository = new Mock<IAccountVerificationRepository>();
            _userRepository = new Mock<IUserRepository>();
            _responseFactory = new Mock<IResponseFactory>();

            _responseFactory.Setup(x => x.CreateSuccess())
                .Returns(() => new ResponseModel { Success = true });

            _responseFactory.Setup(x => x.CreateFailure(It.IsAny<string>()))
                .Returns((string error) => new ResponseModel { Success = false, Error = error });

            _handler = new ResolveAccountVerificationHandler(_accountVerificationRepository.Object, _userRepository.Object,
                _responseFactory.Object, _fileService.Object);
        }

        [Fact]
        public async Task Handle_VerificationAccepted_Success()
        {
            var fileDeleted = false;
            var userVerified = false;
            var verificationDeleted = false;

            _accountVerificationRepository.Setup(x => x.GetEntityByIdAsync(It.IsAny<long>()))
                .ReturnsAsync(new AccountVerification { Id = 1 });

            _accountVerificationRepository.Setup(x => x.DeleteByIdAsync(It.IsAny<long>()))
                .Callback(() => verificationDeleted = true);

            _userRepository.Setup(x => x.UpdateAsync(It.IsAny<UpdateUserRequest>()))
                .Callback((UpdateUserRequest request) => userVerified = request.Verified!.Value);

            _fileService.Setup(x => x.RemoveVerificationDocumentAsync(It.IsAny<string>()))
                .Callback(() => fileDeleted = true);

            var command = new ResolveAccountVerificationCommand
            {
                Accepted = true,
                Id = 1,
                UserId = 2
            };

            var res = await _handler.Handle(command, default);

            Assert.True(res.Success);
            Assert.True(fileDeleted);
            Assert.True(verificationDeleted);
            Assert.Equal(command.Accepted, userVerified);
        }

        [Fact]
        public async Task Handle_VerificationDenied_Success()
        {
            var fileDeleted = false;
            var verificationDeleted = false;

            _accountVerificationRepository.Setup(x => x.GetEntityByIdAsync(It.IsAny<long>()))
                .ReturnsAsync(new AccountVerification { Id = 1 });

            _accountVerificationRepository.Setup(x => x.DeleteByIdAsync(It.IsAny<long>()))
                .Callback(() => verificationDeleted = true);

            _fileService.Setup(x => x.RemoveVerificationDocumentAsync(It.IsAny<string>()))
                .Callback(() => fileDeleted = true);

            var command = new ResolveAccountVerificationCommand
            {
                Accepted = false,
                Id = 1,
                UserId = 2
            };

            var res = await _handler.Handle(command, default);

            Assert.True(res.Success);
            Assert.True(fileDeleted);
            Assert.True(verificationDeleted);
        }

        [Fact]
        public async Task Handle_ExceptionThrown_Failure()
        {
            const string Error = "err";

            _accountVerificationRepository.Setup(x => x.GetEntityByIdAsync(It.IsAny<long>()))
                .ThrowsAsync(new Exception(Error));

            var res = await _handler.Handle(new ResolveAccountVerificationCommand(), default);

            Assert.False(res.Success);
            Assert.Equal(Error, res.Error);
        }
    }
}
