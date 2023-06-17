using Instagram.Application;
using Instagram.Application.Abstraction;
using Instagram.Database.Repository;
using Instagram.Domain.Entity;
using Instagram.Models.PostTag.Request;
using Instagram.Models.Response;
using Moq;

namespace Instagram.Tests.Unit.ServiceTests
{
    public class PostTagServiceTests 
    {
        private readonly Mock<IPostTagRepository> _postTagRepository;
        private readonly Mock<IPostTagFactory> _postTagFactory;
        private readonly Mock<IResponseFactory> _responseFactory;
        private readonly PostTagService _service;

        public PostTagServiceTests()
        {
            _postTagRepository = new Mock<IPostTagRepository>();
            _postTagFactory = new Mock<IPostTagFactory>();
            _responseFactory = new Mock<IResponseFactory>();

            _service = new PostTagService(_postTagRepository.Object, _postTagFactory.Object, _responseFactory.Object);
        }

        [Fact]
        public async Task AddAsync_Success()
        {
            var tags = new List<PostTag>();

            _postTagRepository.Setup(x => x.InsertAsync(It.IsAny<PostTag>()))
                .Callback((PostTag tag) => tags.Add(tag));

            _postTagFactory.Setup(x => x.CreateMany(It.IsAny<AddPostTagRequest>()))
                .Returns((AddPostTagRequest request) => request.Tags.Select(x => new PostTag { PostId = request.PostId, Tag = x }));

            _responseFactory.Setup(x => x.CreateSuccess())
                .Returns(new ResponseModel { Success = true });

            var request = new AddPostTagRequest { PostId = 1, Tags = new string[] { "a", "b" } };

            var res = await _service.AddAsync(request);

            Assert.True(res.Success);
            Assert.Equivalent(request.Tags, tags.Select(x => x.Tag));
        }

        [Fact]
        public async Task AddAsync_ExceptionThrown_Failure()
        {
            const string Error = "err";

            _postTagRepository.Setup(x => x.InsertAsync(It.IsAny<PostTag>()))
                .Callback(() => throw new Exception(Error));

            _postTagFactory.Setup(x => x.CreateMany(It.IsAny<AddPostTagRequest>()))
                .Returns((AddPostTagRequest request) => request.Tags.Select(x => new PostTag { PostId = request.PostId, Tag = x }));

            _responseFactory.Setup(x => x.CreateFailure(It.IsAny<string>()))
                .Returns((string err) => new ResponseModel { Success = false, Error = err });

            var request = new AddPostTagRequest { PostId = 1, Tags = new string[] { "a", "b" } };

            var res = await _service.AddAsync(request);

            Assert.False(res.Success);
            Assert.Equal(Error, res.Error);
        }

        [Fact]
        public async Task DeleteAsync_Success()
        {
            const long PostId = 1;
            const string Tag = "a";

            var tags = new List<PostTag>
            {
                new PostTag { PostId = PostId, Tag = Tag },
                new PostTag { PostId = 2, Tag = "b" },
                new PostTag { PostId = 3, Tag = "c" },
                new PostTag { PostId = PostId, Tag = "d" },
            };

            _postTagRepository.Setup(x => x.DeleteAsync(It.IsAny<long>(), It.IsAny<string>()))
                .Callback((long postId, string tag) => tags.RemoveAll(x => x.PostId == postId && x.Tag == tag));

            _responseFactory.Setup(x => x.CreateSuccess())
               .Returns(new ResponseModel { Success = true });

            var res = await _service.DeleteAsync(PostId, Tag);

            Assert.True(res.Success);
            Assert.DoesNotContain(tags, x => x.PostId == PostId && x.Tag == Tag);
            Assert.Equal(3, tags.Count);
        }

        [Fact]
        public async Task DeleteAsync_ExceptionThrown_Failure()
        {
            const string Error = "Err";

            _postTagRepository.Setup(x => x.DeleteAsync(It.IsAny<long>(), It.IsAny<string>()))
                .Callback(() => throw new Exception(Error));

            _responseFactory.Setup(x => x.CreateFailure(It.IsAny<string>()))
                .Returns((string err) => new ResponseModel { Success = false, Error = err });

            var res = await _service.DeleteAsync(1, "a");

            Assert.False(res.Success);
            Assert.Equal(Error, res.Error);
        }
    }
}
