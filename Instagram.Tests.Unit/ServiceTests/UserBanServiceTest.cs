using Instagram.Application;
using Instagram.Application.Abstraction;
using Instagram.Database.Repository;
using Instagram.Domain.Entity;
using Instagram.Models.Response;
using Instagram.Models.UserBan.Request;
using NSubstitute;
using NSubstitute.ExceptionExtensions;
using NSubstitute.Exceptions;

namespace Instagram.Tests.Unit.ServiceTests
{
    public class UserBanServiceTest
    {
        private readonly IUserBanRepository _userBanRepository;
        private readonly IUserBanFactory _userBanFactory;
        private readonly IResponseFactory _responseFactory;
        private readonly UserBanService _service;

        public UserBanServiceTest()
        {
            _userBanRepository = Substitute.For<IUserBanRepository>();
            _userBanFactory = Substitute.For<IUserBanFactory>();
            _responseFactory = ResponseFactoryMock.Create();

            _service = new UserBanService(_userBanRepository, _userBanFactory, _responseFactory);
        }

        [Fact]
        public async Task AddAsync_Success()
        {
            var bans = new List<UserBan>();

            _userBanRepository.InsertAsync(Arg.Any<UserBan>())
                .Returns(0)
                .AndDoes(info => bans.Add(info.Arg<UserBan>()));

            _userBanFactory.Create(Arg.Any<AddUserBanRequest>())
                .Returns(info => new UserBan
                {
                    EndDate = info.Arg<AddUserBanRequest>().EndDate,
                    UserId = info.Arg<AddUserBanRequest>().UserId
                });

            var request = new AddUserBanRequest { EndDate = 1, UserId = 2 };

            var res = await _service.AddAsync(request);

            Assert.True(res.Success);
            Assert.Contains(bans, x => x.UserId == request.UserId && x.EndDate == request.EndDate);
        }

        [Fact]
        public async Task AddAsync_ExceptionThrown_Failure()
        {
            const string Error = "Err";

            _userBanFactory.Create(Arg.Any<AddUserBanRequest>())
                .Throws(new Exception(Error));

            var request = new AddUserBanRequest { EndDate = 1, UserId = 2 };

            var res = await _service.AddAsync(request);

            Assert.False(res.Success);
            Assert.Equal(Error, res.Error);
        }

        [Fact]
        public async Task DeleteAsync_Success()
        {
            const long Id = 2;
            var bans = new List<UserBan>
            {
                new UserBan { Id = 1, },
                new UserBan { Id = Id, },
                new UserBan { Id = 1, },
            };

            _userBanRepository.DeleteByIdAsync(Arg.Any<long>())
                .Returns(Task.CompletedTask)
                .AndDoes(info => bans.RemoveAll(b => b.Id == info.Arg<long>()));

            var res = await _service.DeleteAsync(Id);

            Assert.True(res.Success);
            Assert.DoesNotContain(bans, x => x.Id == Id);
        }

        [Fact]
        public async Task DeleteAsync_ExceptionThrown_Failure()
        {
            const string Error = "Err";

            _userBanRepository.DeleteByIdAsync(Arg.Any<long>())
                .ThrowsAsync(new Exception(Error));

            var res = await _service.DeleteAsync(2137);

            Assert.False(res.Success);
            Assert.Equal(Error, res.Error);
        }
    }
}
