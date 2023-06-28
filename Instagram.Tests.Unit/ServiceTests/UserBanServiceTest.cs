using Instagram.Application;
using Instagram.Application.Abstraction;
using Instagram.Database.Repository;
using Instagram.Domain.Entity;
using Instagram.Models.Response;
using Instagram.Models.UserBan.Request;
using Moq;

namespace Instagram.Tests.Unit.ServiceTests
{
    public class UserBanServiceTest
    {
        private readonly Mock<IUserBanRepository> _userBanRepository;
        private readonly Mock<IUserBanFactory> _userBanFactory;
        private readonly Mock<IResponseFactory> _responseFactory;
        private readonly UserBanService _service;

        public UserBanServiceTest()
        {
            _userBanRepository = new Mock<IUserBanRepository>();
            _userBanFactory = new Mock<IUserBanFactory>();
            _responseFactory = new Mock<IResponseFactory>();

            _service = new UserBanService(_userBanRepository.Object, _userBanFactory.Object, _responseFactory.Object);
        }

        [Fact]
        public async Task AddAsync_Success()
        {
            var bans = new List<UserBan>();

            _userBanRepository.Setup(x => x.InsertAsync(It.IsAny<UserBan>()))
                .Callback((UserBan ban) => bans.Add(ban));

            _userBanFactory.Setup(x => x.Create(It.IsAny<AddUserBanRequest>()))
                .Returns((AddUserBanRequest request) => new UserBan { UserId = request.UserId, EndDate = request.EndDate });

            _responseFactory.Setup(x => x.CreateSuccess())
                .Returns(new ResponseModel { Success = true });

            var request = new AddUserBanRequest { EndDate = 1, UserId = 2 };

            var res = await _service.AddAsync(request);

            Assert.True(res.Success);
            Assert.Contains(bans, x => x.UserId == request.UserId && x.EndDate == request.EndDate);
        }

        [Fact]
        public async Task AddAsync_ExceptionThrown_Failure()
        {
            const string Error = "Err";

            _userBanRepository.Setup(x => x.InsertAsync(It.IsAny<UserBan>()))
                .Callback((UserBan ban) => throw new Exception(Error));

            _userBanFactory.Setup(x => x.Create(It.IsAny<AddUserBanRequest>()))
                .Returns((AddUserBanRequest request) => new UserBan { UserId = request.UserId, EndDate = request.EndDate });

            _responseFactory.Setup(x => x.CreateFailure(It.IsAny<string>()))
                .Returns((string err) => new ResponseModel { Success = false, Error = err });

            var request = new AddUserBanRequest { EndDate = 1, UserId = 2 };

            var res = await _service.AddAsync(request);

            Assert.False(res.Success);
            Assert.Equal(Error, res.Error);
        }
    }
}
