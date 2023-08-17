using Instagram.Application;
using Instagram.Application.Abstraction;
using Instagram.Database.Repository;
using Instagram.Domain.Entity;
using Instagram.Models.Response;
using Instagram.Models.UserFollow;
using Instagram.Models.UserFollow.Request;
using NSubstitute;
using NSubstitute.ExceptionExtensions;

namespace Instagram.Tests.Unit.ServiceTests
{
    public class UserFollowServiceTests
    {
        private readonly IUserFollowRepository _userFollowRepository;
        private readonly IUserFollowFactory _userFollowFactory;
        private readonly IResponseFactory _responseFactory;
        private readonly UserFollowService _service;

        public UserFollowServiceTests()
        {
            _userFollowRepository = Substitute.For<IUserFollowRepository>();
            _userFollowFactory = Substitute.For<IUserFollowFactory>();
            _responseFactory = ResponseFactoryMock.Create();

            _service = new UserFollowService(_userFollowRepository, _userFollowFactory, _responseFactory);
        }

        [Fact]
        public async Task GetAsync_ReturnsModels()
        {
            var models = new List<UserFollowModel>
            {
                new UserFollowModel(),
                new UserFollowModel(),
                new UserFollowModel(),
            };

            _userFollowRepository.GetAsync(Arg.Any<GetUserFollowRequest>())
                .Returns(models);

            var res = await _service.GetAsync(new GetUserFollowRequest());

            Assert.Equal(models, res);
        }

        [Fact]
        public async Task AddAsync_FollowAdded_Success()
        {
            var follows = new List<UserFollow>();

            _userFollowFactory.Create(Arg.Any<AddUserFollowRequest>())
                .Returns(info => new UserFollow { FollowedUserId = info.Arg<AddUserFollowRequest>().FollowedUserId });

            _userFollowRepository.InsertAsync(Arg.Any<UserFollow>())
                .Returns(Task.CompletedTask)
                .AndDoes(info => follows.Add(info.Arg<UserFollow>()));

            var request = new AddUserFollowRequest { FollowedUserId = 1 };
            var res = await _service.AddAsync(request);

            Assert.True(res.Success);
            Assert.Contains(follows, x => x.FollowedUserId == request.FollowedUserId);
        }

        [Fact]
        public async Task AddAsync_ExceptionThrown_Fail()
        {
            const string Error = "Error";

            _userFollowFactory.Create(Arg.Any<AddUserFollowRequest>())
                .Throws(new Exception(Error));

            var request = new AddUserFollowRequest { FollowedUserId = 1 };
            var res = await _service.AddAsync(request);

            Assert.False(res.Success);
            Assert.Equal(Error, res.Error);
        }

        [Fact]
        public async Task DeleteAsync_FollowDeleted_Success()
        {
            const long FollowerIdToDelete = 1;
            const long FollowedIdToDelete = 2;
            var follows = new List<UserFollow> 
            {
                new UserFollow { FollowedUserId = 1, FollowingUserId = 3 },
                new UserFollow { FollowedUserId = 2, FollowingUserId = 1 },
                new UserFollow { FollowedUserId = FollowerIdToDelete, FollowingUserId = FollowedIdToDelete },
            };

            _userFollowRepository.DeleteAsync(Arg.Any<long>(), Arg.Any<long>())
                .Returns(Task.CompletedTask)
                .AndDoes(info => follows.RemoveAll(f => f.FollowingUserId == info.ArgAt<long>(0) && f.FollowedUserId == info.ArgAt<long>(1)));

            var res = await _service.DeleteAsync(FollowerIdToDelete, FollowedIdToDelete);

            Assert.True(res.Success);
            Assert.DoesNotContain(follows, f => f.FollowedUserId == FollowedIdToDelete && f.FollowingUserId == FollowerIdToDelete);
        }

        [Fact]
        public async Task DeleteAsync_ExceptionThrown_Fail()
        {
            const string Error = "error";

            _userFollowRepository.DeleteAsync(Arg.Any<long>(), Arg.Any<long>())
                .ThrowsAsync(new Exception(Error));

            var res = await _service.DeleteAsync(1, 2);

            Assert.False(res.Success);
            Assert.Equal(Error, res.Error);
        }

        [Fact]
        public async Task GetCountAsync_ReturnsCount()
        {
            const int Count = 10;

            _userFollowRepository.GetCountAsync(Arg.Any<GetUserFollowRequest>()).Returns(Count);

            var res = await _service.GetCountAsync(new GetUserFollowRequest());

            Assert.Equal(Count, res);
        }
    }
}
