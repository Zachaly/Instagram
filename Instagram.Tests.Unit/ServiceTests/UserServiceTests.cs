using Instagram.Application;
using Instagram.Application.Abstraction;
using Instagram.Database.Repository;
using Instagram.Domain.Entity;
using Instagram.Models.Response;
using Instagram.Models.User;
using Instagram.Models.User.Request;
using Moq;

namespace Instagram.Tests.Unit.ServiceTests
{
    public class UserServiceTests
    {
        private readonly Mock<IUserRepository> _userRepository;
        private readonly Mock<IResponseFactory> _responseFactory;
        private readonly UserService _service;

        public UserServiceTests()
        {
            _userRepository = new Mock<IUserRepository>();
            _responseFactory = new Mock<IResponseFactory>();
            _service = new UserService(_userRepository.Object, _responseFactory.Object);
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

            _userRepository.Setup(x => x.GetAsync(It.IsAny<GetUserRequest>())).ReturnsAsync(users);

            var res = await _service.GetAsync(new GetUserRequest());

            Assert.Equivalent(users, res);
        }

        [Fact]
        public async Task GetByIdAsync_ReturnsModel() 
        {
            var user = new UserModel { Id = 1, Name = "test" };

            _userRepository.Setup(x => x.GetByIdAsync(It.IsAny<long>())).ReturnsAsync(user);

            var res = await _service.GetByIdAsync(0);

            Assert.Equivalent(user, res);
        }

        [Fact]
        public async Task UpdateAsync_Success()
        {
            var user = new User { Id = 1, Bio = "Bio" };

            _userRepository.Setup(x => x.UpdateAsync(It.IsAny<UpdateUserRequest>()))
                .Callback((UpdateUserRequest request) =>
                {
                    user.Bio = request.Bio;
                });

            _responseFactory.Setup(x => x.CreateSuccess())
                .Returns(new ResponseModel { Success = true });

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
            _userRepository.Setup(x => x.UpdateAsync(It.IsAny<UpdateUserRequest>()))
                .Callback((UpdateUserRequest request) =>
                {
                    throw new Exception(Error);
                });

            _responseFactory.Setup(x => x.CreateFailure(It.IsAny<string>()))
                .Returns((string err) => new ResponseModel { Success = false, Error = err });

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
