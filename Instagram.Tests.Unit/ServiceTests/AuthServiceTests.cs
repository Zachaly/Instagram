using Instagram.Application;
using Instagram.Domain.Entity;
using Microsoft.Extensions.Configuration;
using Moq;
using System.IdentityModel.Tokens.Jwt;

namespace Instagram.Tests.Unit.ServiceTests
{
    public class AuthServiceTests
    {
        private readonly Mock<IConfiguration> _config;
        private readonly AuthService _service;

        public AuthServiceTests()
        {
            _config = new Mock<IConfiguration>();
            _config.SetupGet(x => x[It.Is<string>(s => s == "Auth:Audience")]).Returns("https://localhost");
            _config.SetupGet(x => x[It.Is<string>(s => s == "Auth:Issuer")]).Returns("https://localhost");
            _config.SetupGet(x => x[It.Is<string>(s => s == "Auth:SecretKey")]).Returns("supersecretkeyloooooooooooooooooooooooooooooooong");
            _service = new AuthService(_config.Object);
        }

        [Fact]
        public async Task HashPassword_PasswordHashed()
        {
            const string Password = "zaq1@WSX";

            var hash = await _service.HashPasswordAsync(Password);

            Assert.NotEqual(Password, hash);
        }

        [Fact]
        public async Task VerifyPasswordAsync_CorrectPassword_ReturnsTrue()
        {
            const string Password = "zaq1@WSX";

            var hash = await _service.HashPasswordAsync(Password);

            var res = await _service.VerifyPasswordAsync(Password, hash);

            Assert.True(res);
        }

        [Fact]
        public async Task VerifyPasswordAsync_WrongPassword_ReturnsTrue()
        {
            const string Password = "zaq1@WSX";

            var hash = await _service.HashPasswordAsync(Password);

            var res = await _service.VerifyPasswordAsync("xsw2!QAZ", hash);

            Assert.False(res);
        }

        [Fact]
        public async Task GenerateTokenAsync_GeneratesValidJwt()
        {
            var user = new User
            {
                Id = 1,
                Email = "mail",
            };

            var token = await _service.GenerateTokenAsync(user);

            var decodedToken = new JwtSecurityTokenHandler().ReadJwtToken(token);

            Assert.Contains(decodedToken.Claims, x => x.Value == user.Id.ToString());
            Assert.Contains(decodedToken.Claims, x => x.Value == user.Email);
            Assert.Contains(decodedToken.Audiences, x => x == _config.Object["Auth:Audience"]);
            Assert.Equal(decodedToken.Issuer, _config.Object["Auth:Issuer"]);
        }
    }
}
