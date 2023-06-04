using Instagram.Database.Repository;
using Instagram.Database.Sql;
using Instagram.Domain.Entity;
using Instagram.Models.User.Request;

namespace Instagram.Tests.Integration.DatabaseTests
{
    public class UserRepositoryTests : RepositoryTest
    {
        private readonly UserRepository _repository;

        public UserRepositoryTests() : base() 
        {
            _repository = new UserRepository(new SqlQueryBuilder(), _connectionFactory);
        }

        [Fact]
        public async Task GetAsync_EmptyRequest_ReturnsAllUsers()
        {
            var users = FakeDataFactory.GenerateUsers(5);
            foreach (var user in users)
            {
                var query = new SqlQueryBuilder().BuildInsert("User", user).Build();
                ExecuteQuery(query, user);
            }

            var userIds = GetFromDatabase<long>("SELECT [Id] FROM [User]");

            var res = await _repository.GetAsync(new GetUserRequest());

            Assert.Equivalent(userIds, res.Select(x => x.Id));
            Assert.Equal(userIds.Count(), res.Count());
        }

        [Fact]
        public async Task GetAsync_RequestWithSingleProperty_ReturnsSpecifiedUsers()
        {
            var users = FakeDataFactory.GenerateUsers(10);
            foreach (var user in users)
            {
                var query = new SqlQueryBuilder().BuildInsert("User", user).Build();
                ExecuteQuery(query, user);
            }

            var request = new GetUserRequest
            {
                Gender = users.First().Gender
            };

            var userIds = GetFromDatabase<long>("SELECT [Id] FROM [User] WHERE [Gender]=@Gender", request);

            var res = await _repository.GetAsync(request);

            Assert.Equivalent(userIds, res.Select(x => x.Id));
            Assert.Equal(userIds.Count(), res.Count());
        }

        [Fact]
        public async Task GetAsync_RequestWithMultipleProperties_ReturnsSpecifiedUsers()
        {
            var users = FakeDataFactory.GenerateUsers(10);
            foreach (var user in users)
            {
                var query = new SqlQueryBuilder().BuildInsert("User", user).Build();
                ExecuteQuery(query, user);
            }

            var request = new GetUserRequest
            {
                Gender = users.First().Gender,
                Bio = ""
            };

            var userIds = GetFromDatabase<long>("SELECT [Id] FROM [User] WHERE [Gender]=@Gender AND [Bio]=@Bio", request);

            var res = await _repository.GetAsync(request);

            Assert.Equivalent(userIds, res.Select(x => x.Id));
            Assert.Equal(userIds.Count(), res.Count());
        }

        [Fact]
        public async Task InsertUser_UserAddedToDatabase()
        {
            var user = new User
            {
                Bio = "",
                Email = "email",
                Gender = 0,
                Name = "name",
                Nickname = "nick",
                PasswordHash = "hash"
            };

            var id = await _repository.InsertAsync(user);

            var users = GetFromDatabase<User>("SELECT * FROM [User]");

            Assert.Single(users);
            Assert.Contains(users, u => u.Id == id 
                && u.Name == user.Name 
                && u.Email == user.Email 
                && u.Gender == user.Gender
                && u.Nickname == user.Nickname
                && u.PasswordHash == user.PasswordHash);
        }

        [Fact]
        public async Task GetEntityByEmail_ReturnsCorrectUser()
        {
            var users = FakeDataFactory.GenerateUsers(5);
            foreach (var user in users)
            {
                var query = new SqlQueryBuilder().BuildInsert("User", user).Build();
                ExecuteQuery(query, user);
            }

            var testUser = GetFromDatabase<User>("SELECT * FROM [User]").First();
            var res = await _repository.GetEntityByEmailAsync(testUser.Email);

            Assert.Equal(testUser.Id, res.Id);
        }

        [Fact]
        public async Task DeleteByIdAsync_DeletesCorrectEntity()
        {
            foreach (var user in FakeDataFactory.GenerateUsers(5))
            {
                var query = new SqlQueryBuilder().BuildInsert("User", user).Build();
                ExecuteQuery(query, user);
            }

            var testUser = GetFromDatabase<User>("SELECT * FROM [User]").First();

            await _repository.DeleteByIdAsync(testUser.Id);

            var users = GetFromDatabase<User>("SELECT * FROM [User]");

            Assert.DoesNotContain(users, u => u.Id == testUser.Id);
            Assert.Equal(4, users.Count());
        }

        [Fact]
        public async Task GetByIdAsync_ReturnsCorrectEntity()
        {
            foreach (var user in FakeDataFactory.GenerateUsers(5))
            {
                var query = new SqlQueryBuilder().BuildInsert("User", user).Build();
                ExecuteQuery(query, user);
            }

            var testUser = GetFromDatabase<User>("SELECT * FROM [User]").Last();

            var res = await _repository.GetByIdAsync(testUser.Id);

            Assert.Equal(testUser.Id, res.Id);
            Assert.Equal(testUser.Name, res.Name);
            Assert.Equal(testUser.Nickname, res.Nickname);
            Assert.Equal(testUser.Gender, res.Gender);
            Assert.Equal(testUser.Bio, res.Bio);
        }

        [Fact]
        public async Task UpdateAsync_UpdatesCorrectUser()
        {
            foreach (var user in FakeDataFactory.GenerateUsers(5))
            {
                var query = new SqlQueryBuilder().BuildInsert("User", user).Build();
                ExecuteQuery(query, user);
            }

            var users = GetFromDatabase<User>("SELECT * FROM [User]");

            var userToUpdate = users.First();

            var request = new UpdateUserRequest { Id = userToUpdate.Id, Name = "New name of the user", Bio = "New bio" };

            await _repository.UpdateAsync(request);

            var updatedUsers = GetFromDatabase<User>("SELECT * FROM [User]");

            Assert.Contains(updatedUsers, user => user.Id == request.Id && user.Name == request.Name && user.Bio == request.Bio);
            Assert.Equal(users.Count(), updatedUsers.Count());
        }

        [Fact]
        public async Task GetAsync_PaginationApplied_ReturnsCorrectUsers()
        {
            foreach (var user in FakeDataFactory.GenerateUsers(10))
            {
                var query = new SqlQueryBuilder().BuildInsert("User", user).Build();
                ExecuteQuery(query, user);
            }

            var userIds = GetFromDatabase<long>("SELECT Id FROM [User] ORDER BY Id");

            var request = new GetUserRequest
            {
                PageIndex = 1,
                PageSize = 5
            };

            var res = await _repository.GetAsync(request);

            Assert.Equivalent(userIds.Skip(5).Take(5), res.Select(x => x.Id));
        }

        [Fact]
        public async Task GetAsync_WithSkipPagination_ReturnsAllUsers()
        {
            foreach (var user in FakeDataFactory.GenerateUsers(10))
            {
                var query = new SqlQueryBuilder().BuildInsert("User", user).Build();
                ExecuteQuery(query, user);
            }

            var userIds = GetFromDatabase<long>("SELECT Id FROM [User] ORDER BY Id");

            var request = new GetUserRequest
            {
                PageIndex = 1,
                PageSize = 5,
                SkipPagination = true
            };

            var res = await _repository.GetAsync(request);

            Assert.Equivalent(userIds, res.Select(x => x.Id));
            Assert.Equal(userIds.Count(), res.Count());
        }
    }
}
