using Instagram.Application;
using Instagram.Application.Abstraction;
using Instagram.Database.Repository;
using Instagram.Domain.Entity;
using Instagram.Models.Response;
using Instagram.Models.UserClaim.Request;
using NSubstitute;
using NSubstitute.ExceptionExtensions;
using NSubstitute.Exceptions;

namespace Instagram.Tests.Unit.ServiceTests
{
    public class UserClaimServiceTests 
    {
        private readonly IUserClaimRepository _userClaimRepository;
        private readonly IUserClaimFactory _userClaimFactory;
        private readonly IResponseFactory _responseFactory;
        private readonly UserClaimService _service;

        public UserClaimServiceTests()
        {
            _userClaimRepository = Substitute.For<IUserClaimRepository>();
            _userClaimFactory = Substitute.For<IUserClaimFactory>();
            _responseFactory = ResponseFactoryMock.Create();

            _service = new UserClaimService(_userClaimRepository, _userClaimFactory, _responseFactory);
        }

        [Fact]
        public async Task AddAsync_Success()
        {
            var claims = new List<UserClaim>();

            _userClaimFactory.Create(Arg.Any<AddUserClaimRequest>())
                .Returns(info => new UserClaim { Value = info.Arg<AddUserClaimRequest>().Value });

            _userClaimRepository.InsertAsync(Arg.Any<UserClaim>())
                .Returns(Task.CompletedTask)
                .AndDoes(info => claims.Add(info.Arg<UserClaim>()));

            var request = new AddUserClaimRequest { Value = "val" };

            var res = await _service.AddAsync(request);

            Assert.True(res.Success);
            Assert.Contains(claims, x => x.Value == request.Value);
        }

        [Fact]
        public async Task AddAsync_ExceptionThrown_Fail()
        {

            const string Error = "Err";

            _userClaimFactory.Create(Arg.Any<AddUserClaimRequest>())
                .Throws(new Exception(Error));

            var request = new AddUserClaimRequest { Value = "val" };

            var res = await _service.AddAsync(request);

            Assert.False(res.Success);
            Assert.Equal(Error, res.Error);
        }

        [Fact]
        public async Task DeleteAsync_Success()
        {
            _userClaimRepository.DeleteAsync(Arg.Any<long>(), Arg.Any<string>()).Returns(Task.CompletedTask);

            var res = await _service.DeleteAsync(1, "val");

            Assert.True(res.Success);
            Assert.Equal(1, _userClaimRepository.GetMethodCallsNumber(nameof(_userClaimRepository.DeleteAsync)));
        }

        [Fact]
        public async Task DeleteAsync_ExceptionThrown_Failure()
        {
            const string Error = "err";

            _userClaimRepository.DeleteAsync(Arg.Any<long>(), Arg.Any<string>())
                .ThrowsAsync(new Exception(Error));

            var res = await _service.DeleteAsync(1, "val");

            Assert.False(res.Success);
            Assert.Equal(Error, res.Error);
        }
    }
}
