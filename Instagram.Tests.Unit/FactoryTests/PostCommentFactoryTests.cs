using Instagram.Application;
using Instagram.Models.PostComment.Request;

namespace Instagram.Tests.Unit.FactoryTests
{
    public class PostCommentFactoryTests
    {
        private readonly PostCommentFactory _factory;

        public PostCommentFactoryTests()
        {
            _factory = new PostCommentFactory();
        }

        [Fact]
        public void Create_CreatesValidEntity()
        {
            var request = new AddPostCommentRequest
            {
                Content = "content",
                PostId = 1,
                UserId = 2
            };

            var comment = _factory.Create(request);

            Assert.Equal(request.Content, comment.Content);
            Assert.Equal(request.UserId, comment.UserId);
            Assert.Equal(request.PostId, comment.PostId);
        }
    }
}
