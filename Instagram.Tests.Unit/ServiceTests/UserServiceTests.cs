using Instagram.Application;
using Instagram.Application.Abstraction;
using Instagram.Database.Repository;
using Instagram.Domain.Entity;
using Instagram.Models.Response;
using Instagram.Models.User;
using Instagram.Models.User.Request;
using NSubstitute;
using NSubstitute.ExceptionExtensions;

namespace Instagram.Tests.Unit.ServiceTests
{
    public class UserServiceTests
    {
        private readonly IUserRepository _userRepository;
        private readonly IResponseFactory _responseFactory;
        private readonly UserService _service;

        public UserServiceTests()
        {
            _userRepository = Substitute.For<IUserRepository>();
            _responseFactory = ResponseFactoryMock.Create();
            _service = new UserService(_userRepository, _responseFactory);
        }

        [Fact]
        public async Task GetAsync_ReturnsModels()
        {
            var users = new List<UserModel>
            {
                new UserModel { Id = 1 },
                new UserModel { Id = 2 },
                new UserModel { Id = 3 },
                new UserModel { Id = 4 },
            };

            _userRepository.GetAsync(Arg.Any<GetUserRequest>()).Returns(users);

            var res = await _service.GetAsync(new GetUserRequest());

            Assert.Equivalent(users, res);
        }

        [Fact]
        public async Task GetByIdAsync_ReturnsModel() 
        {
            var user = new UserModel { Id = 1, Name = "test" };

            _userRepository.GetByIdAsync(Arg.Any<long>()).Returns(user);

            var res = await _service.GetByIdAsync(0);

            Assert.Equivalent(user, res);
        }

        [Fact]
        public async Task UpdateAsync_Success()
        {
            var user = new User { Id = 1, Bio = "Bio" };

            _userRepository.UpdateAsync(Arg.Any<UpdateUserRequest>())
                .Returns(Task.CompletedTask)
                .AndDoes(info => user.Bio = info.Arg<UpdateUserRequest>().Bio);

            var request = new UpdateUserRequest
            {
                Bio = "new bio"
            };

            var res = await _service.UpdateAsync(request);

            Assert.True(res.Success);
            Assert.Equal(request.Bio, user.Bio);
        }

        [Fact]
        public async Task UpdateAsync_ExceptionThrown_Failure()
        {
            const string Error = "Error";
            _userRepository.UpdateAsync(Arg.Any<UpdateUserRequest>())
                .ThrowsAsync(new Exception(Error));

            var request = new UpdateUserRequest
            {
                Bio = "new bio"
            };

            var res = await _service.UpdateAsync(request);

            Assert.False(res.Success);
            Assert.Equal(Error, res.Error);
        }
    }
}
