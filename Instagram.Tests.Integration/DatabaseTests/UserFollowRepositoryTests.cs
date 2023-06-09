using Instagram.Database.Repository;
using Instagram.Database.Sql;
using Instagram.Domain.Entity;
using Instagram.Models.UserFollow.Request;

namespace Instagram.Tests.Integration.DatabaseTests
{
    public class UserFollowRepositoryTests : RepositoryTest
    {
        private readonly UserFollowRepository _repository;

        public UserFollowRepositoryTests() : base()
        {
            _repository = new UserFollowRepository(new SqlQueryBuilder(), _connectionFactory);
        }

        [Fact]
        public async Task GetAsync_RequestWithConditionalJoin_JoinsProperColumns()
        {
            foreach (var user in FakeDataFactory.GenerateUsers(2))
            {
                var query = new SqlQueryBuilder().BuildInsert("User", user).Build();
                ExecuteQuery(query, user);
            }

            var users = GetFromDatabase<User>("SELECT * FROM [User]");

            var follows = new List<UserFollow>
            {
                new UserFollow { FollowedUserId = users.First().Id, FollowingUserId = users.Last().Id },
                new UserFollow { FollowedUserId = users.Last().Id, FollowingUserId = users.First().Id }
            };

            foreach(var follow in follows)
            {
                var query = new SqlQueryBuilder().BuildInsert("UserFollow", follow).Build();
                ExecuteQuery(query, follow);
            }

            var request = new GetUserFollowRequest
            {
                JoinFollowed = true
            };

            var res = await _repository.GetAsync(request);

            Assert.Equal(2, res.Count());
            Assert.Contains(res, x => x.FollowedUserId == users.First().Id && x.UserName == users.First().Nickname);
            Assert.Contains(res, x => x.FollowedUserId == users.Last().Id && x.UserName == users.Last().Nickname);
        }

        [Fact]
        public async Task GetAsync_RequestWithExclusiveConditionalJoins_OneJoinApplied()
        {
            foreach (var user in FakeDataFactory.GenerateUsers(2))
            {
                var query = new SqlQueryBuilder().BuildInsert("User", user).Build();
                ExecuteQuery(query, user);
            }

            var users = GetFromDatabase<User>("SELECT * FROM [User]");

            var follows = new List<UserFollow>
            {
                new UserFollow { FollowedUserId = users.First().Id, FollowingUserId = users.Last().Id },
                new UserFollow { FollowedUserId = users.Last().Id, FollowingUserId = users.First().Id }
            };

            foreach (var follow in follows)
            {
                var query = new SqlQueryBuilder().BuildInsert("UserFollow", follow).Build();
                ExecuteQuery(query, follow);
            }

            var request = new GetUserFollowRequest
            {
                JoinFollowed = true,
                JoinFollower = true,
            };

            var res = await _repository.GetAsync(request);

            Assert.Equal(2, res.Count());
            Assert.All(res, x => Assert.NotNull(x.UserName));
        }

        [Fact]
        public async Task DeleteAsync_DeletesProperEntities()
        {
            const long FollowerIdToDelete = 2;
            const long FollowedIdToDelete = 3;

            var follows = new UserFollow[]
            {
                new UserFollow { FollowedUserId = 1, FollowingUserId = 2 },
                new UserFollow { FollowedUserId = FollowedIdToDelete, FollowingUserId = FollowerIdToDelete },
                new UserFollow { FollowedUserId = 3, FollowingUserId = 4 },
                new UserFollow { FollowedUserId = 5, FollowingUserId = 6 },
            };

            foreach (var follow in follows)
            {
                var query = new SqlQueryBuilder().BuildInsert("UserFollow", follow).Build();
                ExecuteQuery(query, follow);
            }

            await _repository.DeleteAsync(FollowerIdToDelete, FollowedIdToDelete);

            var updatedFollows = GetFromDatabase<UserFollow>("SELECT * FROM UserFollow");

            Assert.DoesNotContain(updatedFollows, x => x.FollowedUserId == FollowedIdToDelete && x.FollowingUserId == FollowerIdToDelete);
        }
    }
}
