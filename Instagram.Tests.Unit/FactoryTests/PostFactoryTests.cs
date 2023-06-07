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
            const string FileName = "file";

            var post = _factory.Create(request);

            Assert.Equal(request.Content, post.Content);
            Assert.Equal(request.CreatorId, post.CreatorId);
            Assert.InRange(post.Created, DateTimeOffset.UtcNow.ToUnixTimeSeconds() - 50, DateTimeOffset.UtcNow.ToUnixTimeSeconds() + 50);
        }
    }
}
