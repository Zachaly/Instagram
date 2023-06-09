using Instagram.Application;
using Instagram.Application.Abstraction;
using Instagram.Database.Repository;
using Instagram.Domain.Entity;
using Instagram.Models.Response;
using Instagram.Models.UserFollow;
using Instagram.Models.UserFollow.Request;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Instagram.Tests.Unit.ServiceTests
{
    public class UserFollowServiceTests
    {
        private readonly Mock<IUserFollowRepository> _userFollowRepository;
        private readonly Mock<IUserFollowFactory> _userFollowFactory;
        private readonly Mock<IResponseFactory> _responseFactory;
        private readonly UserFollowService _service;

        public UserFollowServiceTests()
        {
            _userFollowRepository = new Mock<IUserFollowRepository>();
            _userFollowFactory = new Mock<IUserFollowFactory>();
            _responseFactory = new Mock<IResponseFactory>();
            _service = new UserFollowService(_userFollowRepository.Object, _userFollowFactory.Object, _responseFactory.Object);
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

            _userFollowRepository.Setup(x => x.GetAsync(It.IsAny<GetUserFollowRequest>()))
                .ReturnsAsync(models);

            var res = await _service.GetAsync(new GetUserFollowRequest());

            Assert.Equal(models, res);
        }

        [Fact]
        public async Task AddAsync_FollowAdded_Success()
        {
            var follows = new List<UserFollow>();

            _userFollowFactory.Setup(x => x.Create(It.IsAny<AddUserFollowRequest>()))
                .Returns((AddUserFollowRequest req) => new UserFollow { FollowedUserId = req.FollowedUserId });

            _userFollowRepository.Setup(x => x.InsertAsync(It.IsAny<UserFollow>()))
                .Callback((UserFollow follow) => follows.Add(follow));

            _responseFactory.Setup(x => x.CreateSuccess())
                .Returns(new ResponseModel { Success = true });

            var request = new AddUserFollowRequest { FollowedUserId = 1 };
            var res = await _service.AddAsync(request);

            Assert.True(res.Success);
            Assert.Contains(follows, x => x.FollowedUserId == request.FollowedUserId);
        }

        [Fact]
        public async Task AddAsync_ExceptionThrown_Fail()
        {
            const string Error = "Error";

            _userFollowFactory.Setup(x => x.Create(It.IsAny<AddUserFollowRequest>()))
                .Returns((AddUserFollowRequest req) => new UserFollow { FollowedUserId = req.FollowedUserId });

            _userFollowRepository.Setup(x => x.InsertAsync(It.IsAny<UserFollow>()))
                .Callback(() => throw new Exception(Error));

            _responseFactory.Setup(x => x.CreateFailure(It.IsAny<string>()))
                .Returns((string err) => new ResponseModel { Success = true, Error = err });

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

            _userFollowRepository.Setup(x => x.DeleteAsync(It.IsAny<long>(), It.IsAny<long>()))
                .Callback((long followerId, long followedId)
                    => follows.RemoveAll(f => f.FollowedUserId == followedId && f.FollowingUserId == followerId));

            _responseFactory.Setup(x => x.CreateSuccess())
                .Returns(new ResponseModel { Success = true });

            var res = await _service.DeleteAsync(FollowerIdToDelete, FollowedIdToDelete);

            Assert.True(res.Success);
            Assert.DoesNotContain(follows, f => f.FollowedUserId == FollowedIdToDelete && f.FollowingUserId == FollowerIdToDelete);
        }

        [Fact]
        public async Task DeleteAsync_ExceptionThrown_Fail()
        {
            const string Error = "error";

            _userFollowRepository.Setup(x => x.DeleteAsync(It.IsAny<long>(), It.IsAny<long>()))
                .Callback(() => throw new Exception(Error));

            _responseFactory.Setup(x => x.CreateSuccess())
                .Returns(new ResponseModel { Success = true });

            var res = await _service.DeleteAsync(1, 2);

            Assert.False(res.Success);
            Assert.Equal(Error, res.Error);
        }
    }
}
