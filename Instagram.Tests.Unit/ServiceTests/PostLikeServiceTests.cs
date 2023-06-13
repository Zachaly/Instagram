using Instagram.Application;
using Instagram.Application.Abstraction;
using Instagram.Database.Repository;
using Instagram.Domain.Entity;
using Instagram.Models.PostLike;
using Instagram.Models.PostLike.Request;
using Instagram.Models.Response;
using Moq;

namespace Instagram.Tests.Unit.ServiceTests
{
    public class PostLikeServiceTests
    {
        private readonly Mock<IPostLikeFactory> _postLikeFactory;
        private readonly Mock<IPostLikeRepository> _postLikeRepository;
        private readonly Mock<IResponseFactory> _responseFactory;
        private readonly PostLikeService _service;

        public PostLikeServiceTests()
        {
            _postLikeFactory = new Mock<IPostLikeFactory>();
            _postLikeRepository = new Mock<IPostLikeRepository>();
            _responseFactory = new Mock<IResponseFactory>();
            _service = new PostLikeService(_postLikeFactory.Object, _postLikeRepository.Object, _responseFactory.Object);
        }

        [Fact]
        public async Task GetAsync_ReturnsModels()
        {
            var likes = new List<PostLikeModel>
            {
                new PostLikeModel { UserId = 1 },
                new PostLikeModel { UserId = 2 },
                new PostLikeModel { UserId = 3 },
                new PostLikeModel { UserId = 4 },
                new PostLikeModel { UserId = 5 },
            };

            _postLikeRepository.Setup(x => x.GetAsync(It.IsAny<GetPostLikeRequest>()))
                .ReturnsAsync(likes);

            var res = await _service.GetAsync(new GetPostLikeRequest());

            Assert.Equivalent(res, likes);
        }

        [Fact]
        public async Task AddAsync_AddsLike()
        {
            var likes = new List<PostLike>();

            _postLikeFactory.Setup(x => x.Create(It.IsAny<AddPostLikeRequest>()))
                .Returns((AddPostLikeRequest req) => new PostLike { PostId = req.PostId, UserId = req.UserId });

            _postLikeRepository.Setup(x => x.InsertAsync(It.IsAny<PostLike>()))
                .Callback((PostLike like) => likes.Add(like));

            _responseFactory.Setup(x => x.CreateSuccess())
                .Returns(new ResponseModel { Success = true });

            var request = new AddPostLikeRequest { PostId = 1, UserId = 2 };
            var res = await _service.AddAsync(request);

            Assert.True(res.Success);
            Assert.Contains(likes, x => x.PostId == request.PostId && x.UserId == request.UserId);
        }

        [Fact]
        public async Task AddAsync_ExceptionThrown_Failure()
        {
            const string Error = "err";

            _postLikeFactory.Setup(x => x.Create(It.IsAny<AddPostLikeRequest>()))
                .Returns((AddPostLikeRequest req) => new PostLike { PostId = req.PostId, UserId = req.UserId });

            _postLikeRepository.Setup(x => x.InsertAsync(It.IsAny<PostLike>()))
                .Callback((PostLike like) => throw new Exception(Error));

            _responseFactory.Setup(x => x.CreateFailure(It.IsAny<string>()))
                .Returns((string err) => new ResponseModel { Success = false, Error = err });

            var request = new AddPostLikeRequest { PostId = 1, UserId = 2 };
            var res = await _service.AddAsync(request);

            Assert.False(res.Success);
            Assert.Equal(Error, res.Error);
        }

        [Fact]
        public async Task GetCountAsync_ReturnsCount()
        {
            const int Count = 10;

            _postLikeRepository.Setup(x => x.GetCountAsync(It.IsAny<GetPostLikeRequest>()))
                .ReturnsAsync(Count);

            var res = await _service.GetCountAsync(new GetPostLikeRequest());

            Assert.Equal(Count, res);
        }

        [Fact]
        public async Task DeleteAsync_RemovesLike()
        {
            const long UserIdToDelete = 2;
            const long PostIdToDelete = 3;

            var likes = new List<PostLike>
            {
                new PostLike { PostId = 1, UserId = 2 },
                new PostLike { PostId = 2, UserId = 3 },
                new PostLike { PostId = PostIdToDelete, UserId = UserIdToDelete },
                new PostLike { PostId = 4, UserId = 4 },
            };

            _postLikeRepository.Setup(x => x.DeleteAsync(It.IsAny<long>(), It.IsAny<long>()))
                .Callback((long postId, long userId) => likes.RemoveAll(x => x.PostId == postId && x.UserId == userId));

            _responseFactory.Setup(x => x.CreateSuccess())
                .Returns(new ResponseModel { Success = true });

            var res = await _service.DeleteAsync(PostIdToDelete, UserIdToDelete);

            Assert.True(res.Success);
            Assert.DoesNotContain(likes, x => x.PostId == PostIdToDelete && x.UserId == UserIdToDelete);
        }

        [Fact]
        public async Task DeleteAsync_ExceptionThrown_Failure()
        {
            const string Error = "error";

            _postLikeRepository.Setup(x => x.DeleteAsync(It.IsAny<long>(), It.IsAny<long>()))
                .Callback((long postId, long userId) => throw new Exception(Error));

            _responseFactory.Setup(x => x.CreateFailure(It.IsAny<string>()))
                .Returns((string err) => new ResponseModel { Success = false, Error = err });

            var res = await _service.DeleteAsync(1, 2);

            Assert.True(res.Success);
            Assert.Equal(Error, res.Error);
        }
    }
}
