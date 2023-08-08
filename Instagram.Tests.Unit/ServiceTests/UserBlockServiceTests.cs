using Instagram.Application;
using Instagram.Application.Abstraction;
using Instagram.Database.Repository;
using Instagram.Domain.Entity;
using Instagram.Models.Response;
using Instagram.Models.UserBlock.Request;
using Moq;

namespace Instagram.Tests.Unit.ServiceTests
{
    public class UserBlockServiceTests
    {
        private readonly Mock<IUserBlockRepository> _userBlockRepository;
        private readonly Mock<IUserBlockFactory> _userBlockFactory;
        private readonly Mock<IResponseFactory> _responseFactory;
        private readonly UserBlockService _service;

        public UserBlockServiceTests()
        {
            _userBlockRepository = new Mock<IUserBlockRepository>();
            _userBlockFactory = new Mock<IUserBlockFactory>();
            _responseFactory = new Mock<IResponseFactory>();

            _responseFactory.Setup(x => x.CreateSuccess())
                .Returns(new ResponseModel { Success = true });

            _responseFactory.Setup(x => x.CreateSuccess(It.IsAny<long>()))
                .Returns((long id) => new ResponseModel { Success = true, NewEntityId = id });

            _responseFactory.Setup(x => x.CreateFailure(It.IsAny<string>()))
                .Returns((string error) => new ResponseModel { Success = false, Error = error });

            _service = new UserBlockService(_userBlockRepository.Object, _userBlockFactory.Object, _responseFactory.Object);
        }

        [Fact]
        public async Task AddAsync_AddsUserBlock()
        {
            var blocks = new List<UserBlock>();

            const long NewId = 2;

            _userBlockRepository.Setup(x => x.InsertAsync(It.IsAny<UserBlock>()))
                .Callback((UserBlock block) => blocks.Add(block))
                .ReturnsAsync(NewId);

            _userBlockFactory.Setup(x => x.Create(It.IsAny<AddUserBlockRequest>()))
                .Returns((AddUserBlockRequest request) => new UserBlock { BlockedUserId = request.BlockedUserId });

            var request = new AddUserBlockRequest
            {
                BlockedUserId = 1,
            };

            var res = await _service.AddAsync(request);

            Assert.True(res.Success);
            Assert.Contains(blocks, x => x.BlockedUserId == request.BlockedUserId);
            Assert.Equal(NewId, res.NewEntityId);
        }

        [Fact]
        public async Task AddAsync_ExceptionThrown_Failure()
        {
            const string Error = "err";

            _userBlockFactory.Setup(x => x.Create(It.IsAny<AddUserBlockRequest>()))
                .Throws(new Exception(Error));

            var res = await _service.AddAsync(new AddUserBlockRequest());

            Assert.False(res.Success);
            Assert.Equal(Error, res.Error);
        }

        [Fact]
        public async Task DeleteByIdAsync_DeletesUserBlock()
        {
            const long IdToDelete = 2;

            var blocks = new List<UserBlock>
            {
                new UserBlock { Id = 1 },
                new UserBlock { Id = IdToDelete },
                new UserBlock { Id = 3 }
            };

            _userBlockRepository.Setup(x => x.DeleteByIdAsync(It.IsAny<long>()))
                .Callback((long id) => blocks.RemoveAll(x => x.Id == id));

            var res = await _service.DeleteByIdAsync(IdToDelete);

            Assert.True(res.Success);
            Assert.DoesNotContain(blocks, x => x.Id == IdToDelete);
        }

        [Fact]
        public async Task DeleteByIdAsync_ExceptionThrown_Failure()
        {
            const string Error = "err";

            _userBlockRepository.Setup(x => x.DeleteByIdAsync(It.IsAny<long>()))
                .ThrowsAsync(new Exception(Error));

            var res = await _service.DeleteByIdAsync(2137);

            Assert.False(res.Success);
            Assert.Equal(Error, res.Error);
        }
    }
}
