using Instagram.Database.Repository;
using Instagram.Database.Sql;
using Instagram.Domain.Entity;
using Instagram.Models.Post.Request;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Instagram.Tests.Integration.DatabaseTests
{
    public class PostRepositoryTests : RepositoryTest
    {
        private readonly PostRepository _repository;

        public PostRepositoryTests() : base()
        {
            _repository = new PostRepository(new SqlQueryBuilder(), _connectionFactory);
        }

        [Fact]
        public async Task GetAsync_ReturnsPosts_WithJoinedColumns()
        {
            foreach(var user in FakeDataFactory.GenerateUsers(2))
            {
                var query = new SqlQueryBuilder().BuildInsert("User", user).Build();
                ExecuteQuery(query, user);
            }

            var users = GetFromDatabase<User>("SELECT * FROM [User]");

            var user1 = users.First();
            var user2 = users.Last();

            var postsToInsert = FakeDataFactory.GeneratePosts(5, user1.Id);
            postsToInsert.AddRange(FakeDataFactory.GeneratePosts(5, user2.Id));

            foreach (var post in postsToInsert)
            {
                var query = new SqlQueryBuilder().BuildInsert("Post", post).Build();
                ExecuteQuery(query, post);
            }

            var postIds = GetFromDatabase<long>("SELECT Id FROM [Post]");

            var request = new GetPostRequest();

            var res = await _repository.GetAsync(request);

            Assert.All(res.Where(x => x.CreatorId == user1.Id), post => Assert.Equal(user1.Nickname, post.CreatorName));
            Assert.All(res.Where(x => x.CreatorId == user2.Id), post => Assert.Equal(user2.Nickname, post.CreatorName));
            Assert.Equivalent(postIds, res.Select(x => x.Id));
        }
    }
}
