using Instagram.Application;
using Instagram.Models.PostLike.Request;

namespace Instagram.Tests.Unit.FactoryTests
{
    public class PostLikeFactoryTests
    {
        private readonly PostLikeFactory _factory;

        public PostLikeFactoryTests()
        {
            _factory = new PostLikeFactory();
        }

        [Fact]
        public void Create_CreatesProperEntity()
        {
            var request = new AddPostLikeRequest { PostId = 1, UserId = 2 };

            var like = _factory.Create(request);

            Assert.Equal(request.PostId, like.PostId);
            Assert.Equal(request.UserId, like.UserId);
        }
    }
}
