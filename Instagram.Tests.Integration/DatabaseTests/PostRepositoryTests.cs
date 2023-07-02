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
            Insert("User", FakeDataFactory.GenerateUsers(2));

            var users = GetFromDatabase<User>("SELECT * FROM [User]");

            var user1 = users.First();
            var user2 = users.Last();

            var postsToInsert = FakeDataFactory.GeneratePosts(5, user1.Id);
            postsToInsert.AddRange(FakeDataFactory.GeneratePosts(5, user2.Id));

            Insert("Post", postsToInsert);

            var postIds = GetFromDatabase<long>("SELECT Id FROM [Post]");

            var postTags = new List<PostTag>();

            foreach(var postId in postIds)
            {
                Insert("PostImage", FakeDataFactory.GeneratePostImages(postId, 2));
                postTags.AddRange(FakeDataFactory.GeneratePostTags(postId, 2));
            }

            Insert("PostTag", postTags);

            var images = GetFromDatabase<PostImage>("SELECT * FROM [PostImage]");

            var request = new GetPostRequest();

            var res = await _repository.GetAsync(request);

            Assert.All(res.Where(x => x.CreatorId == user1.Id), post => Assert.Equal(user1.Nickname, post.CreatorName));
            Assert.All(res.Where(x => x.CreatorId == user2.Id), post => Assert.Equal(user2.Nickname, post.CreatorName));
            Assert.All(res, post =>
            {
                Assert.Equivalent(images.Where(x => x.PostId == post.Id).Select(x => x.Id), post.ImageIds);
                Assert.Equivalent(postTags.Where(x => x.PostId == post.Id).Select(x => x.Tag), post.Tags);
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

            Insert("PostImage", FakeDataFactory.GeneratePostImages(post.Id, 10));

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
            Insert("User", FakeDataFactory.GenerateUsers(5));

            var userIds = GetFromDatabase<long>("SELECT Id FROM [User]");

            var postsToInsert = new List<Post>();

            foreach(var id in userIds)
            {
                postsToInsert.AddRange(FakeDataFactory.GeneratePosts(2, id));
            }

            Insert("Post", postsToInsert);

            var postIds = GetFromDatabase<long>("SELECT Id FROM Post");

            foreach (var id in postIds)
            {
                var image = new PostImage { File = "file", PostId = id };
                Insert("PostImage", image);
            }

            var likes = FakeDataFactory.GeneratePostLikes(postIds, userIds);

            Insert("PostLike", likes);

            var result = await _repository.GetAsync(new GetPostRequest());

            Assert.All(result, post => Assert.Equal(likes.Count(x => x.PostId == post.Id), post.LikeCount));
        }

        [Fact]
        public async Task GetAsync_WithSearchTag_ReturnsProperPosts()
        {
            Insert("User", FakeDataFactory.GenerateUsers(1));

            var userId = GetFromDatabase<long>("SELECT Id FROM [User]").First();

            Insert("Post", FakeDataFactory.GeneratePosts(10, userId));

            var postIds = GetFromDatabase<long>("SELECT Id FROM Post").Take(3);

            var images = postIds.Select(x => new PostImage { File = "", PostId = x });
            Insert("PostImage", images);

            const string SearchTag = "tag";
            var tags = postIds.Select(x => new PostTag { PostId = x, Tag = SearchTag });

            Insert("PostTag", tags);

            var result = await _repository.GetAsync(new GetPostRequest { SearchTag = SearchTag });

            Assert.Equivalent(postIds, result.Select(x => x.Id));
        }
    }
}
