using Instagram.Application;
using Instagram.Database.Repository;
using Instagram.Models.Post;
using Instagram.Models.Post.Request;
using NSubstitute;

namespace Instagram.Tests.Unit.ServiceTests
{
    public class PostServiceTests
    {
        private readonly IPostRepository _postRepository;
        private readonly PostService _service;

        public PostServiceTests()
        {
            _postRepository = Substitute.For<IPostRepository>();
            _service = new PostService(_postRepository);
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

            _postRepository.GetAsync(Arg.Any<GetPostRequest>()).Returns(posts);

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

            _postRepository.GetByIdAsync(Arg.Any<long>()).Returns(post);

            var res = await _service.GetByIdAsync(1);

            Assert.Equal(post, res);
        }

        [Fact]
        public async Task GetCountAsync_ReturnsCount()
        {
            const int Count = 20;

            _postRepository.GetCountAsync(Arg.Any<GetPostRequest>()).Returns(Count);

            var res = await _service.GetCountAsync(new GetPostRequest());

            Assert.Equal(Count, res);
        }
    }
}
