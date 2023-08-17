using Instagram.Application;
using Instagram.Application.Abstraction;
using Instagram.Database.Repository;
using Instagram.Domain.Entity;
using Instagram.Models.PostLike;
using Instagram.Models.PostLike.Request;
using Instagram.Models.Response;
using NSubstitute;
using NSubstitute.ExceptionExtensions;

namespace Instagram.Tests.Unit.ServiceTests
{
    public class PostLikeServiceTests
    {
        private readonly IPostLikeFactory _postLikeFactory;
        private readonly IPostLikeRepository _postLikeRepository;
        private readonly IResponseFactory _responseFactory;
        private readonly PostLikeService _service;

        public PostLikeServiceTests()
        {
            _postLikeFactory = Substitute.For<IPostLikeFactory>();
            _postLikeRepository = Substitute.For<IPostLikeRepository>();
            _responseFactory = ResponseFactoryMock.Create();
            _service = new PostLikeService(_postLikeFactory, _postLikeRepository, _responseFactory);
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

            _postLikeRepository.GetAsync(Arg.Any<GetPostLikeRequest>()).Returns(likes);

            var res = await _service.GetAsync(new GetPostLikeRequest());

            Assert.Equivalent(res, likes);
        }

        [Fact]
        public async Task AddAsync_AddsLike()
        {
            var likes = new List<PostLike>();

            _postLikeFactory.Create(Arg.Any<AddPostLikeRequest>())
                .Returns(info => new PostLike
                {
                    PostId = info.Arg<AddPostLikeRequest>().PostId,
                    UserId = info.Arg<AddPostLikeRequest>().UserId,
                });

            _postLikeRepository.InsertAsync(Arg.Any<PostLike>())
                .Returns(Task.CompletedTask)
                .AndDoes(info => likes.Add(info.Arg<PostLike>()));

            var request = new AddPostLikeRequest { PostId = 1, UserId = 2 };
            var res = await _service.AddAsync(request);

            Assert.True(res.Success);
            Assert.Contains(likes, x => x.PostId == request.PostId && x.UserId == request.UserId);
        }

        [Fact]
        public async Task AddAsync_ExceptionThrown_Failure()
        {
            const string Error = "err";

            _postLikeFactory.Create(Arg.Any<AddPostLikeRequest>())
                .Throws(new Exception(Error));

            var request = new AddPostLikeRequest { PostId = 1, UserId = 2 };
            var res = await _service.AddAsync(request);

            Assert.False(res.Success);
            Assert.Equal(Error, res.Error);
        }

        [Fact]
        public async Task GetCountAsync_ReturnsCount()
        {
            const int Count = 10;

            _postLikeRepository.GetCountAsync(Arg.Any<GetPostLikeRequest>()).Returns(Count);

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

            _postLikeRepository.DeleteAsync(Arg.Any<long>(), Arg.Any<long>())
                .Returns(Task.CompletedTask)
                .AndDoes(info => likes.RemoveAll(x => x.PostId == info.ArgAt<long>(0) && x.UserId == info.ArgAt<long>(1)));

            var res = await _service.DeleteAsync(PostIdToDelete, UserIdToDelete);

            Assert.True(res.Success);
            Assert.DoesNotContain(likes, x => x.PostId == PostIdToDelete && x.UserId == UserIdToDelete);
        }

        [Fact]
        public async Task DeleteAsync_ExceptionThrown_Failure()
        {
            const string Error = "error";

            _postLikeRepository.DeleteAsync(Arg.Any<long>(), Arg.Any<long>())
                .ThrowsAsync(new Exception(Error));

            var res = await _service.DeleteAsync(1, 2);

            Assert.False(res.Success);
            Assert.Equal(Error, res.Error);
        }
    }
}
