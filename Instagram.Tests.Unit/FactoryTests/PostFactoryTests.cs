using Instagram.Application;
using Instagram.Models.Post.Request;

namespace Instagram.Tests.Unit.FactoryTests
{
    public class PostFactoryTests
    {
        private readonly PostFactory _factory;

        public PostFactoryTests()
        {
            _factory = new PostFactory();
        }

        [Fact]
        public void Create_CreatesValidEntity()
        {
            var request = new AddPostRequest
            {
                Content = "con",
                CreatorId = 1
            };

            var post = _factory.Create(request);

            Assert.Equal(request.Content, post.Content);
            Assert.Equal(request.CreatorId, post.CreatorId);
            Assert.InRange(post.Created, DateTimeOffset.UtcNow.ToUnixTimeSeconds() - 50, DateTimeOffset.UtcNow.ToUnixTimeSeconds() + 50);
        }

        [Fact]
        public void CreateImages_CreatesValidEntities()
        {
            var files = new string[] { "file1", "file2" };
            const long PostId = 1;

            var images = _factory.CreateImages(files, PostId);

            Assert.All(images, image => Assert.Equal(PostId, image.PostId));
            Assert.Equivalent(files, images.Select(x => x.File));
        }
    }
}
