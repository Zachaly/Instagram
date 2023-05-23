using Instagram.Application;
using Instagram.Database.Repository;
using Instagram.Models.User;
using Instagram.Models.User.Request;
using Moq;

namespace Instagram.Tests.Unit.ServiceTests
{
    public class UserServiceTests
    {
        private readonly Mock<IUserRepository> _userRepository;
        private readonly UserService _service;

        public UserServiceTests()
        {
            _userRepository = new Mock<IUserRepository>();

            _service = new UserService(_userRepository.Object);
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

    }
}
