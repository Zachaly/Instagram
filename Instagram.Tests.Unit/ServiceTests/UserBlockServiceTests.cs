using Instagram.Application;
using Instagram.Application.Abstraction;
using Instagram.Database.Repository;
using Instagram.Domain.Entity;
using Instagram.Models.UserBlock.Request;
using NSubstitute;
using NSubstitute.ExceptionExtensions;

namespace Instagram.Tests.Unit.ServiceTests
{
    public class UserBlockServiceTests
    {
        private readonly IUserBlockRepository _userBlockRepository;
        private readonly IUserBlockFactory _userBlockFactory;
        private readonly IResponseFactory _responseFactory;
        private readonly UserBlockService _service;

        public UserBlockServiceTests()
        {
            _userBlockRepository = Substitute.For<IUserBlockRepository>();
            _userBlockFactory = Substitute.For<IUserBlockFactory>();
            _responseFactory = ResponseFactoryMock.Create();

            _service = new UserBlockService(_userBlockRepository, _userBlockFactory, _responseFactory);
        }

        [Fact]
        public async Task AddAsync_AddsUserBlock()
        {
            var blocks = new List<UserBlock>();

            const long NewId = 2;

            _userBlockRepository.InsertAsync(Arg.Any<UserBlock>())
                .Returns(NewId)
                .AndDoes(info => blocks.Add(info.Arg<UserBlock>()));

            _userBlockFactory.Create(Arg.Any<AddUserBlockRequest>())
                .Returns(info => new UserBlock { BlockedUserId = info.Arg<AddUserBlockRequest>().BlockedUserId });

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

            _userBlockFactory.Create(Arg.Any<AddUserBlockRequest>()).Throws(new Exception(Error));

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

            _userBlockRepository.DeleteByIdAsync(Arg.Any<long>())
                .Returns(Task.CompletedTask)
                .AndDoes(info => blocks.RemoveAll(b => b.Id == info.Arg<long>()));

            var res = await _service.DeleteByIdAsync(IdToDelete);

            Assert.True(res.Success);
            Assert.DoesNotContain(blocks, x => x.Id == IdToDelete);
        }

        [Fact]
        public async Task DeleteByIdAsync_ExceptionThrown_Failure()
        {
            const string Error = "err";

            _userBlockRepository.DeleteByIdAsync(Arg.Any<long>()).ThrowsAsync(new Exception(Error));

            var res = await _service.DeleteByIdAsync(2137);

            Assert.False(res.Success);
            Assert.Equal(Error, res.Error);
        }
    }
}
