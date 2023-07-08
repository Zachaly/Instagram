using Instagram.Application.Command;
using Instagram.Database.Sql;
using Instagram.Domain.Entity;
using Instagram.Domain.Enum;
using Instagram.Models.Response;
using Instagram.Models.User;
using Instagram.Models.User.Request;
using Instagram.Tests.Integration.ApiTests.Infrastructure;
using System.Net;
using System.Net.Http.Headers;
using System.Net.Http.Json;

namespace Instagram.Tests.Integration.ApiTests
{
    public class UserControllerTests : ApiTest
    {
        const string Endpoint = "/api/user";

        [Fact]
        public async Task RegisterAsync_ValidData_UserCreated()
        {
            var request = new RegisterCommand
            {
                Email = "email@email.com",
                Gender = Gender.Man,
                Name = "test name",
                Nickname = "testnickname",
                Password = "password"
            };

            var response = await _httpClient.PostAsJsonAsync(Endpoint, request);

            var users = GetFromDatabase<User>("SELECT * FROM [User] WHERE Nickname!='__admin__'");

            Assert.Equal(HttpStatusCode.Created, response.StatusCode);
            Assert.Single(users);
            Assert.Contains(users, x => x.Email == request.Email 
                && x.Gender == request.Gender 
                && x.Name == request.Name 
                && x.Nickname == request.Nickname);
        }

        [Fact]
        public async Task RegisterAsync_InvalidRequest_ReturnsErrors()
        {
            var request = new RegisterCommand
            {
                Email = "emai",
                Gender = Gender.Man,
                Name = "test name",
                Nickname = "testnickname",
                Password = "password"
            };

            var response = await _httpClient.PostAsJsonAsync(Endpoint, request);
            var content = await ReadErrorResponse(response);

            var users = GetFromDatabase<User>("SELECT * FROM [User] WHERE Nickname!='__admin__'");
            
            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
            Assert.Contains(content.ValidationErrors.Keys, x => x == "Email");
            Assert.DoesNotContain(users, x => x.Email == request.Email
                && x.Gender == request.Gender
                && x.Name == request.Name
                && x.Nickname == request.Nickname);
        }

        [Fact]
        public async Task RegisterAsync_EmailTaken_Failure()
        {
            var existingUser = new User
            {
                Email = "email@email.com",
                Gender = Gender.Man,
                Name = "name",
                Nickname = "nickname",
                PasswordHash = "hash",
                Bio = "",
            };

            var query = new SqlQueryBuilder().BuildInsert("User", existingUser).Build();

            ExecuteQuery(query, existingUser);

            var request = new RegisterCommand
            {
                Name = "new name",
                Email = existingUser.Email,
                Gender = Gender.NotSpecified,
                Nickname = "new nickname",
                Password = "password"
            };

            var response = await _httpClient.PostAsJsonAsync(Endpoint, request);

            var users = GetFromDatabase<User>("SELECT * FROM [User] WHERE Nickname!='__admin__'");

            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
            Assert.Single(users);
            Assert.DoesNotContain(users, x => x.Name == request.Name);
        }

        [Fact]
        public async Task LoginAsync_ValidData_Success()
        {
            var registerRequest = new RegisterCommand
            {
                Email = "email@email.com",
                Gender = Gender.Man,
                Name = "test name",
                Nickname = "testnickname",
                Password = "password"
            };

            await _httpClient.PostAsJsonAsync(Endpoint, registerRequest);

            var loginRequest = new LoginCommand
            {
                Email = registerRequest.Email,
                Password = registerRequest.Password,
            };

            var response = await _httpClient.PostAsJsonAsync($"{Endpoint}/login", loginRequest);
            var content = await response.Content.ReadFromJsonAsync<LoginResponse>();

            var user = GetFromDatabase<User>("SELECT * FROM [User] WHERE [Id]=@Id", new { Id = content.UserId });

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.Single(user);
            Assert.NotEmpty(content.AuthToken);
        }

        [Fact]
        public async Task LoginAsync_InvalidPassword_Failure()
        {
            var registerRequest = new RegisterCommand
            {
                Email = "email@email.com",
                Gender = Gender.Man,
                Name = "test name",
                Nickname = "testnickname",
                Password = "password"
            };

            await _httpClient.PostAsJsonAsync(Endpoint, registerRequest);

            var loginRequest = new LoginCommand
            {
                Email = registerRequest.Email,
                Password = "drowssap",
            };

            var response = await _httpClient.PostAsJsonAsync($"{Endpoint}/login", loginRequest);
            var content = await response.Content.ReadFromJsonAsync<DataResponseModel<LoginResponse>>();

            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
            Assert.NotEmpty(content.Error);
            Assert.False(content.Success);
        }

        [Fact]
        public async Task LoginAsync_UserDoesNotExist_Failure()
        {
            var registerRequest = new RegisterCommand
            {
                Email = "email@email.com",
                Gender = Gender.Man,
                Name = "test name",
                Nickname = "testnickname",
                Password = "password"
            };

            await _httpClient.PostAsJsonAsync(Endpoint, registerRequest);

            var loginRequest = new LoginCommand
            {
                Email = "anotheremail@email.com",
                Password = "drowssap",
            };

            var response = await _httpClient.PostAsJsonAsync(Endpoint, loginRequest);

            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        }

        [Fact]
        public async Task GetAsync_WithoutQueryParams_ReturnsUsers()
        {
            var users = FakeDataFactory.GenerateUsers(5);

            Insert("User", users);

            var response = await _httpClient.GetAsync(Endpoint);
            var content = await response.Content.ReadFromJsonAsync<IEnumerable<UserModel>>();

            var userIds = GetFromDatabase<long>("SELECT [Id] FROM [User]");

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.Equal(userIds.Count(), content.Count());
            Assert.Equivalent(userIds, content.Select(x => x.Id));
        }

        [Fact]
        public async Task GetAsync_WithQueryParams_ReturnsUsers()
        {
            var users = FakeDataFactory.GenerateUsers(5);

            Insert("User", users);

            var name = users.First().Nickname;

            var response = await _httpClient.GetAsync($"{Endpoint}?Name={name}");
            var content = await response.Content.ReadFromJsonAsync<IEnumerable<UserModel>>();

            var userIds = GetFromDatabase<long>("SELECT [Id] FROM [User] WHERE [Name]=@Name", new { Name = name });

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.Equal(userIds.Count(), content.Count());
            Assert.Equivalent(userIds, content.Select(x => x.Id));
        }

        [Fact]
        public async Task GetByIdAsync_Success()
        {
            var user = FakeDataFactory.GenerateUsers(1).First();

            var query = new SqlQueryBuilder().BuildInsert("User", user).Build();

            ExecuteQuery(query, user);

            var userId = GetFromDatabase<long>("SELECT [Id] FROM [User] WHERE Nickname!='__admin__'").First();

            var response = await _httpClient.GetAsync($"{Endpoint}/{userId}");
            var content = await response.Content.ReadFromJsonAsync<UserModel>();

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.Equal(user.Name, content.Name);
            Assert.Equal(user.Nickname, content.Nickname);
            Assert.Equal(user.Bio, content.Bio);
            Assert.Equal(user.Gender, content.Gender);
        }

        [Fact]
        public async Task GetByIdAsync_UserNotFound_Fail()
        {
            var response = await _httpClient.GetAsync($"{Endpoint}/2137");

            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        }

        [Fact]
        public async Task UpdateAsync_Success()
        {
            await Authorize();

            Insert("User", FakeDataFactory.GenerateUsers(5));

            var users = GetFromDatabase<User>("SELECT * FROM [User]");
            var userToUpdate = users.Last();

            var request = new UpdateUserRequest
            {
                Id = userToUpdate.Id,
                Bio = "new bio",
            };

            var response = await _httpClient.PatchAsJsonAsync(Endpoint, request);

            var updatedUsers = GetFromDatabase<User>("SELECT * FROM [User]");

            Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);
            Assert.Contains(updatedUsers, user => user.Id == request.Id && user.Bio == request.Bio);
        }

        [Fact]
        public async Task UpdateAsync_InvalidRequest_ReturnsErrors()
        {
            await Authorize();

            Insert("User", FakeDataFactory.GenerateUsers(5));

            var users = GetFromDatabase<User>("SELECT * FROM [User]");
            var userToUpdate = users.Last();

            var request = new UpdateUserRequest
            {
                Id = userToUpdate.Id,
                Name = "a"
            };

            var response = await _httpClient.PatchAsJsonAsync(Endpoint, request);
            var content = await ReadErrorResponse(response);

            var updatedUsers = GetFromDatabase<User>("SELECT * FROM [User]");

            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
            Assert.Contains(content.ValidationErrors.Keys, x => x == "Name");
            Assert.DoesNotContain(updatedUsers, user => user.Id == request.Id && user.Bio == request.Bio);
        }

        [Fact]
        public async Task SearchNickname_ReturnsProperUsers()
        {
            const string SearchNickname = "name";

            var usersToInsert = new User[]
            {
                new User { Nickname = "name", Bio ="", Email = "email@email.com", Name = "normal name", Gender = 0, PasswordHash = "hash" },
                new User { Nickname = "name1", Bio ="", Email = "email@email.com", Name = "normal name", Gender = 0, PasswordHash = "hash" },
                new User { Nickname = "name2", Bio ="", Email = "email@email.com", Name = "normal name", Gender = 0, PasswordHash = "hash" },
                new User { Nickname = "enam4", Bio ="", Email = "email@email.com", Name = "normal name", Gender = 0, PasswordHash = "hash" },
                new User { Nickname = "enam3", Bio ="", Email = "email@email.com", Name = "normal name", Gender = 0, PasswordHash = "hash" },
                new User { Nickname = "name3", Bio ="", Email = "email@email.com", Name = "normal name", Gender = 0, PasswordHash = "hash" },
                new User { Nickname = "eman", Bio ="", Email = "email@email.com", Name = "normal name", Gender = 0, PasswordHash = "hash" },
                new User { Nickname = "eman2", Bio ="", Email = "email@email.com", Name = "normal name", Gender = 0, PasswordHash = "hash" },
            };

            Insert("User", usersToInsert);

            var users = GetFromDatabase<User>("SELECT * FROM [User]");

            var response = await _httpClient.GetAsync($"{Endpoint}?SearchNickname={SearchNickname}");
            var content = await response.Content.ReadFromJsonAsync<IEnumerable<UserModel>>();

            Assert.Equivalent(HttpStatusCode.OK, response.StatusCode);
            Assert.Equivalent(users.Where(u => u.Nickname.Contains(SearchNickname)).Select(x => x.Id), content.Select(x => x.Id));
        }

        [Fact]
        public async Task GetCurrentUser_ReturnsLoginData()
        {
            var registerRequest = new RegisterRequest
            {
                Email = "email@email.com",
                Name = "username",
                Password = "zaq1@WSX",
                Nickname = "nickname",
                Gender = 0,
            };

            await _httpClient.PostAsJsonAsync(Endpoint, registerRequest);

            var loginRequest = new LoginRequest
            {
                Email = registerRequest.Email,
                Password = registerRequest.Password,
            };

            var loginResponse = await _httpClient.PostAsJsonAsync($"{Endpoint}/login", loginRequest);
            var loginContent = await loginResponse.Content.ReadFromJsonAsync<LoginResponse>();

            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue($"Bearer", loginContent.AuthToken);

            var response = await _httpClient.GetAsync($"{Endpoint}/current");
            var content = await response.Content.ReadFromJsonAsync<LoginResponse>();

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.Equal(loginContent.Email, content.Email);
            Assert.Equal(loginContent.UserId, content.UserId);
            Assert.Equivalent(loginContent.Claims, content.Claims);
            Assert.NotEmpty(content.AuthToken);
        }

        [Fact]
        public async Task ChangePassword_ChangesPassword()
        {
            var registerRequest = new RegisterRequest
            {
                Password = "zaq1@WSX",
                Email = "email@email.com",
                Name = "username",
                Nickname = "nickname"
            };

            await _httpClient.PostAsJsonAsync(Endpoint, registerRequest);

            var loginRequest = new LoginRequest
            {
                Email = registerRequest.Email,
                Password = registerRequest.Password,
            };

            var loginResponse = await _httpClient.PostAsJsonAsync($"{Endpoint}/login", loginRequest);
            var loginContent = await loginResponse.Content.ReadFromJsonAsync<LoginResponse>();

            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", loginContent.AuthToken);

            var changePasswordRequest = new ChangePasswordCommand
            {
                NewPassword = "XSW@1qaz",
                OldPassword = "zaq1@WSX",
                UserId = loginContent.UserId,
            };

            var oldHash = GetFromDatabase<string>("SELECT PasswordHash FROM [User] WHERE Id=@Id", new { Id = loginContent.UserId }).First();

            var response = await _httpClient.PatchAsJsonAsync($"{Endpoint}/change-password", changePasswordRequest);

            var newHash = GetFromDatabase<string>("SELECT PasswordHash FROM [User] WHERE Id=@Id", new { Id = loginContent.UserId }).First();

            var loginNewPasswordRequest = new LoginRequest
            {
                Password = changePasswordRequest.NewPassword,
                Email = loginContent.Email,
            };

            var loginWithNewPasswordResponse = await _httpClient.PostAsJsonAsync($"{Endpoint}/login", loginNewPasswordRequest);

            Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);
            Assert.Equal(HttpStatusCode.OK, loginWithNewPasswordResponse.StatusCode);
            Assert.NotEqual(oldHash, newHash);
        }

        [Fact]
        public async Task ChangePassword_InvalidNewPassword_DoesNotChangePassword()
        {
            var registerRequest = new RegisterRequest
            {
                Password = "zaq1@WSX",
                Email = "email@email.com",
                Name = "username",
                Nickname = "nickname"
            };

            await _httpClient.PostAsJsonAsync(Endpoint, registerRequest);

            var loginRequest = new LoginRequest
            {
                Email = registerRequest.Email,
                Password = registerRequest.Password,
            };

            var loginResponse = await _httpClient.PostAsJsonAsync($"{Endpoint}/login", loginRequest);
            var loginContent = await loginResponse.Content.ReadFromJsonAsync<LoginResponse>();

            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", loginContent.AuthToken);

            var changePasswordRequest = new ChangePasswordCommand
            {
                NewPassword = "a",
                OldPassword = "zaq1@WSX",
                UserId = loginContent.UserId,
            };

            var oldHash = GetFromDatabase<string>("SELECT PasswordHash FROM [User] WHERE Id=@Id", new { Id = loginContent.UserId }).First();

            var response = await _httpClient.PatchAsJsonAsync($"{Endpoint}/change-password", changePasswordRequest);
            var content = await ReadErrorResponse(response);

            var newHash = GetFromDatabase<string>("SELECT PasswordHash FROM [User] WHERE Id=@Id", new { Id = loginContent.UserId }).First();

            var loginNewPasswordRequest = new LoginRequest
            {
                Password = changePasswordRequest.NewPassword,
                Email = loginContent.Email,
            };

            var loginWithNewPasswordResponse = await _httpClient.PostAsJsonAsync($"{Endpoint}/login", loginNewPasswordRequest);

            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
            Assert.Equal(HttpStatusCode.BadRequest, loginWithNewPasswordResponse.StatusCode);
            Assert.Contains(content.ValidationErrors.Keys, x => x == "NewPassword");
            Assert.Equal(oldHash, newHash);
        }
    }
}
