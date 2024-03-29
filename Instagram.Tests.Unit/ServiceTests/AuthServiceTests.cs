﻿using Instagram.Application;
using Instagram.Domain.Entity;
using Microsoft.Extensions.Configuration;
using NSubstitute;
using System.IdentityModel.Tokens.Jwt;

namespace Instagram.Tests.Unit.ServiceTests
{
    public class AuthServiceTests
    {
        private readonly IConfiguration _config;
        private readonly AuthService _service;

        public AuthServiceTests()
        {
            _config = Substitute.For<IConfiguration>();
            _config[Arg.Is((string s) => s == "Auth:Audience")].Returns("https://localhost");
            _config[Arg.Is((string s) => s == "Auth:Issuer")].Returns("https://localhost");
            _config[Arg.Is((string s) => s == "Auth:SecretKey")].Returns("supersecretkeyloooooooooooooooooooooooooooooooong");
            _config[Arg.Is((string s) => s == "EncryptionKey")].Returns(new string('a', 24));
            _service = new AuthService(_config);
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

            var claim = new UserClaim { UserId = 1, Value = "claim" };

            var token = await _service.GenerateTokenAsync(user, new List<UserClaim> { claim });

            var decodedToken = new JwtSecurityTokenHandler().ReadJwtToken(token);

            Assert.Contains(decodedToken.Claims, x => x.Value == user.Id.ToString());
            Assert.Contains(decodedToken.Claims, x => x.Value == user.Email);
            Assert.Contains(decodedToken.Audiences, x => x == _config["Auth:Audience"]);
            Assert.Equal(decodedToken.Issuer, _config["Auth:Issuer"]);
            Assert.Contains(decodedToken.Claims, x => x.Value == claim.Value);
        }
    }
}
