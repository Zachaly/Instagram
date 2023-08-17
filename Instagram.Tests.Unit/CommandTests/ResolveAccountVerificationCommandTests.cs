using Instagram.Application.Abstraction;
using Instagram.Application.Command;
using Instagram.Database.Repository;
using Instagram.Domain.Entity;
using Instagram.Models.User.Request;
using NSubstitute;
using NSubstitute.ExceptionExtensions;

namespace Instagram.Tests.Unit.CommandTests
{
    public class ResolveAccountVerificationCommandTests
    {
        private readonly IFileService _fileService;
        private readonly IAccountVerificationRepository _accountVerificationRepository;
        private readonly IUserRepository _userRepository;
        private readonly IResponseFactory _responseFactory;
        private readonly ResolveAccountVerificationHandler _handler;

        public ResolveAccountVerificationCommandTests()
        {
            _fileService = Substitute.For<IFileService>();
            _accountVerificationRepository = Substitute.For<IAccountVerificationRepository>();
            _userRepository = Substitute.For<IUserRepository>();
            _responseFactory = ResponseFactoryMock.Create();

            _handler = new ResolveAccountVerificationHandler(_accountVerificationRepository, _userRepository,
                _responseFactory, _fileService);
        }

        [Fact]
        public async Task Handle_VerificationAccepted_Success()
        {
            var fileDeleted = false;
            var userVerified = false;
            var verificationDeleted = false;

            _accountVerificationRepository.GetEntityByIdAsync(Arg.Any<long>())
                .Returns(new AccountVerification { Id = 1 });

            _accountVerificationRepository.DeleteByIdAsync(Arg.Any<long>()).Returns(Task.CompletedTask);

            _userRepository.UpdateAsync(Arg.Any<UpdateUserRequest>())
                .Returns(Task.CompletedTask)
                .AndDoes(info => userVerified = info.Arg<UpdateUserRequest>().Verified!.Value);

            _fileService.RemoveVerificationDocumentAsync(Arg.Any<string>()).Returns(Task.CompletedTask);

            var command = new ResolveAccountVerificationCommand
            {
                Accepted = true,
                Id = 1
            };

            var res = await _handler.Handle(command, default);

            Assert.True(res.Success);
            Assert.Equal(1, _fileService.GetMethodCallsNumber(nameof(_fileService.RemoveVerificationDocumentAsync)));
            Assert.Equal(1, _accountVerificationRepository.GetMethodCallsNumber(nameof(_accountVerificationRepository.DeleteByIdAsync)));
            Assert.Equal(command.Accepted, userVerified);
        }

        [Fact]
        public async Task Handle_VerificationDenied_Success()
        {
            _accountVerificationRepository.GetEntityByIdAsync(Arg.Any<long>())
                .Returns(new AccountVerification { Id = 1 });

            _accountVerificationRepository.DeleteByIdAsync(Arg.Any<long>()).Returns(Task.CompletedTask);

            _fileService.RemoveVerificationDocumentAsync(Arg.Any<string>()).Returns(Task.CompletedTask);

            var command = new ResolveAccountVerificationCommand
            {
                Accepted = false,
                Id = 1
            };

            var res = await _handler.Handle(command, default);

            Assert.True(res.Success);
            Assert.Equal(1, _fileService.GetMethodCallsNumber(nameof(_fileService.RemoveVerificationDocumentAsync)));
            Assert.Equal(1, _accountVerificationRepository.GetMethodCallsNumber(nameof(_accountVerificationRepository.DeleteByIdAsync)));
        }

        [Fact]
        public async Task Handle_ExceptionThrown_Failure()
        {
            const string Error = "err";

            _accountVerificationRepository.GetEntityByIdAsync(Arg.Any<long>()).ThrowsAsync(new Exception(Error));

            var res = await _handler.Handle(new ResolveAccountVerificationCommand(), default);

            Assert.False(res.Success);
            Assert.Equal(Error, res.Error);
        }
    }
}
