using Instagram.Application;
using Instagram.Database.Repository;
using Instagram.Models.Post;
using Instagram.Models.Post.Request;
using Moq;

namespace Instagram.Tests.Unit.ServiceTests
{
    public class PostServiceTests
    {
        private readonly Mock<IPostRepository> _postRepository;
        private readonly PostService _service;

        public PostServiceTests()
        {
            _postRepository = new Mock<IPostRepository>();
            _service = new PostService(_postRepository.Object);
        }

        [Fact]
        public async Task GetAsync_ReturnsPosts()
        {
            var posts = new List<PostModel>
            {
                new PostModel { Id = 1, },
                new PostModel { Id = 2, },
                new PostModel { Id = 3, },
            };

            _postRepository.Setup(x => x.GetAsync(It.IsAny<GetPostRequest>())).ReturnsAsync(posts);

            var res = await _service.GetAsync(new GetPostRequest());

            Assert.Equivalent(posts, res);
        }

        [Fact]
        public async Task GetByIdAsync_ReturnsPost()
        {
            var post = new PostModel
            {
                Id = 1,
            };

            _postRepository.Setup(x => x.GetByIdAsync(It.IsAny<long>())).ReturnsAsync(post);

            var res = await _service.GetByIdAsync(1);

            Assert.Equal(post, res);
        }
    }
}
