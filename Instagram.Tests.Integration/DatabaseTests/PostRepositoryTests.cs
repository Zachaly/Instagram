using Instagram.Database.Repository;
using Instagram.Database.Sql;
using Instagram.Domain.Entity;
using Instagram.Models.Post.Request;

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

            foreach(var postId in postIds)
            {
                foreach(var image in FakeDataFactory.GeneratePostImages(postId, 2))
                {
                    var query = new SqlQueryBuilder().BuildInsert("PostImage", image).Build();
                    ExecuteQuery(query, image);
                }
            }

            var images = GetFromDatabase<PostImage>("SELECT * FROM [PostImage]");

            var request = new GetPostRequest();

            var res = await _repository.GetAsync(request);

            Assert.All(res.Where(x => x.CreatorId == user1.Id), post => Assert.Equal(user1.Nickname, post.CreatorName));
            Assert.All(res.Where(x => x.CreatorId == user2.Id), post => Assert.Equal(user2.Nickname, post.CreatorName));
            Assert.All(res, post =>
            {
                Assert.Equivalent(images.Where(x => x.PostId == post.Id).Select(x => x.Id), post.ImageIds);
            });
            Assert.Equivalent(postIds, res.Select(x => x.Id));
        }

        [Fact]
        public async Task GetByIdAsync_ImagesJoined()
        {
            var user = FakeDataFactory.GenerateUsers(1).First();

            var insertUserQuery = new SqlQueryBuilder().BuildInsert("User", user).Build();
            ExecuteQuery(insertUserQuery, user);

            user.Id = GetFromDatabase<long>("SELECT Id FROM [User]").First();

            var post = FakeDataFactory.GeneratePosts(1, user.Id).First();
            var insertPostQuery = new SqlQueryBuilder().BuildInsert("Post", post).Build();
            ExecuteQuery(insertPostQuery, post);

            post.Id = GetFromDatabase<long>("SELECT Id FROM Post").First();

            foreach(var image in FakeDataFactory.GeneratePostImages(post.Id, 10))
            {
                var query = new SqlQueryBuilder().BuildInsert("PostImage", image).Build();
                ExecuteQuery(query, image);
            }

            var imageIds = GetFromDatabase<long>("SELECT Id FROM PostImage");

            var res = await _repository.GetByIdAsync(post.Id);

            Assert.Equal(post.Content, res.Content);
            Assert.Equal(post.Created, res.Created);
            Assert.Equal(post.CreatorId, res.CreatorId);
            Assert.Equal(user.Nickname, res.CreatorName);
            Assert.Equivalent(imageIds, res.ImageIds);
        }

        [Fact]
        public async Task GetAsync_ModelsHaveProperLikeCount()
        {
            foreach(var user in FakeDataFactory.GenerateUsers(5))
            {
                Insert("User", user);
            }

            var userIds = GetFromDatabase<long>("SELECT Id FROM [User]");

            var postsToInsert = new List<Post>();

            foreach(var id in userIds)
            {
                postsToInsert.AddRange(FakeDataFactory.GeneratePosts(2, id));
            }

            foreach(var post in postsToInsert)
            {
                Insert("Post", post);
            }

            var postIds = GetFromDatabase<long>("SELECT Id FROM Post");

            foreach (var id in postIds)
            {
                var image = new PostImage { File = "file", PostId = id };
                Insert("PostImage", image);
            }

            var likes = FakeDataFactory.GeneratePostLikes(postIds, userIds);

            foreach (var like in likes)
            {
                Insert("PostLike", like);
            }

            var result = await _repository.GetAsync(new GetPostRequest());

            Assert.All(result, post => Assert.Equal(likes.Count(x => x.PostId == post.Id), post.LikeCount));
        }
    }
}
