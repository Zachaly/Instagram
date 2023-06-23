using Instagram.Application;
using Instagram.Application.Abstraction;
using Instagram.Database.Repository;
using Instagram.Domain.Entity;
using Instagram.Models.Response;
using Instagram.Models.UserClaim.Request;
using Moq;

namespace Instagram.Tests.Unit.ServiceTests
{
    public class UserClaimServiceTests 
    {
        private readonly Mock<IUserClaimRepository> _userClaimRepository;
        private readonly Mock<IUserClaimFactory> _userClaimFactory;
        private readonly Mock<IResponseFactory> _responseFactory;
        private readonly UserClaimService _service;

        public UserClaimServiceTests()
        {
            _userClaimRepository = new Mock<IUserClaimRepository>();
            _userClaimFactory = new Mock<IUserClaimFactory>();
            _responseFactory = new Mock<IResponseFactory>();

            _service = new UserClaimService(_userClaimRepository.Object, _userClaimFactory.Object, _responseFactory.Object);
        }

        [Fact]
        public async Task AddAsync_Success()
        {
            var claims = new List<UserClaim>();

            _userClaimFactory.Setup(x => x.Create(It.IsAny<AddUserClaimRequest>()))
                .Returns((AddUserClaimRequest request) => new UserClaim { Value = request.Value });

            _userClaimRepository.Setup(x => x.InsertAsync(It.IsAny<UserClaim>()))
                .Callback((UserClaim claim) => claims.Add(claim));

            _responseFactory.Setup(x => x.CreateSuccess())
                .Returns(new ResponseModel { Success = true });

            var request = new AddUserClaimRequest { Value = "val" };

            var res = await _service.AddAsync(request);

            Assert.True(res.Success);
            Assert.Contains(claims, x => x.Value == request.Value);
        }

        [Fact]
        public async Task AddAsync_ExceptionThrown_Fail()
        {
            _userClaimFactory.Setup(x => x.Create(It.IsAny<AddUserClaimRequest>()))
                .Returns((AddUserClaimRequest request) => new UserClaim { Value = request.Value });

            const string Error = "Err";

            _userClaimRepository.Setup(x => x.InsertAsync(It.IsAny<UserClaim>()))
                .Callback(() => throw new Exception(Error));

            _responseFactory.Setup(x => x.CreateFailure(It.IsAny<string>()))
                .Returns((string error) => new ResponseModel { Success = false, Error = error });

            var request = new AddUserClaimRequest { Value = "val" };

            var res = await _service.AddAsync(request);

            Assert.False(res.Success);
            Assert.Equal(Error, res.Error);
        }

        [Fact]
        public async Task DeleteAsync_Success()
        {
            _userClaimRepository.Setup(x => x.DeleteAsync(It.IsAny<long>(), It.IsAny<string>()));

            _responseFactory.Setup(x => x.CreateSuccess())
                .Returns(new ResponseModel { Success = true });

            var res = await _service.DeleteAsync(1, "val");

            Assert.True(res.Success);
        }

        [Fact]
        public async Task DeleteAsync_ExceptionThrown_Failure()
        {
            const string Error = "err";
            _userClaimRepository.Setup(x => x.DeleteAsync(It.IsAny<long>(), It.IsAny<string>()))
                .Callback(() => throw new Exception(Error));

            _responseFactory.Setup(x => x.CreateFailure(It.IsAny<string>()))
                .Returns((string error) => new ResponseModel { Success = false, Error = error });

            var res = await _service.DeleteAsync(1, "val");

            Assert.False(res.Success);
            Assert.Equal(Error, res.Error);
        }
    }
}
