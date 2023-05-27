using Instagram.Application;
using Instagram.Models.User.Request;

namespace Instagram.Tests.Unit.FactoryTests
{
    public class UserFactoryTests
    {
        private readonly UserFactory _factory;

        public UserFactoryTests()
        {
            _factory = new UserFactory();
        }

        [Fact]
        public void Create_CreatesValidEntity()
        {
            var request = new RegisterRequest
            {
                Email = "email",
                Gender = Domain.Enum.Gender.Man,
                Name = "name",
                Nickname = "nick",
                Password = "pass"
            };

            const string PasswordHash = "hash";

            var user = _factory.Create(request, PasswordHash);

            Assert.Equal(request.Name, user.Name);
            Assert.Equal(request.Email, user.Email);
            Assert.Equal(request.Gender, user.Gender);
            Assert.Equal(request.Nickname, user.Nickname);
            Assert.Equal(PasswordHash, user.PasswordHash);
        }

        [Fact]
        public void CreateLoginResponse_CreatesProperObject()
        {
            const long UserId = 1;
            const string Token = "token";
            const string Email = "email";

            var response = _factory.CreateLoginResponse(UserId, Token, Email);

            Assert.Equal(UserId, response.UserId);
            Assert.Equal(Email, response.Email);
            Assert.Equal(Token, response.AuthToken);
        }
    }
}
