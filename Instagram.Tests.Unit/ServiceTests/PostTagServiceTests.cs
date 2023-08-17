using Instagram.Application;
using Instagram.Application.Abstraction;
using Instagram.Database.Repository;
using Instagram.Domain.Entity;
using Instagram.Models.PostTag.Request;
using Instagram.Models.Response;
using NSubstitute;
using NSubstitute.ExceptionExtensions;

namespace Instagram.Tests.Unit.ServiceTests
{
    public class PostTagServiceTests 
    {
        private readonly IPostTagRepository _postTagRepository;
        private readonly IPostTagFactory _postTagFactory;
        private readonly IResponseFactory _responseFactory;
        private readonly PostTagService _service;

        public PostTagServiceTests()
        {
            _postTagRepository = Substitute.For<IPostTagRepository>();
            _postTagFactory = Substitute.For<IPostTagFactory>();
            _responseFactory = ResponseFactoryMock.Create();

            _service = new PostTagService(_postTagRepository, _postTagFactory, _responseFactory);
        }

        [Fact]
        public async Task AddAsync_Success()
        {
            var tags = new List<PostTag>();

            _postTagRepository.InsertAsync(Arg.Any<PostTag>())
                .Returns(Task.CompletedTask)
                .AndDoes(info => tags.Add(info.Arg<PostTag>()));

            _postTagFactory.CreateMany(Arg.Any<AddPostTagRequest>())
                .Returns(info => info.Arg<AddPostTagRequest>().Tags.Select(n => new PostTag 
                { 
                    PostId = info.Arg<AddPostTagRequest>().PostId,
                    Tag = n
                }));

            var request = new AddPostTagRequest { PostId = 1, Tags = new string[] { "a", "b" } };

            var res = await _service.AddAsync(request);

            Assert.True(res.Success);
            Assert.Equivalent(request.Tags, tags.Select(x => x.Tag));
        }

        [Fact]
        public async Task AddAsync_ExceptionThrown_Failure()
        {
            const string Error = "err";

            _postTagFactory.CreateMany(Arg.Any<AddPostTagRequest>()).Throws(new Exception(Error));

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

            _postTagRepository.DeleteAsync(Arg.Any<long>(), Arg.Any<string>())
                .Returns(Task.CompletedTask)
                .AndDoes(info => tags.RemoveAll(t => t.PostId == info.Arg<long>() && t.Tag == info.Arg<string>()));

            var res = await _service.DeleteAsync(PostId, Tag);

            Assert.True(res.Success);
            Assert.DoesNotContain(tags, x => x.PostId == PostId && x.Tag == Tag);
            Assert.Equal(3, tags.Count);
        }

        [Fact]
        public async Task DeleteAsync_ExceptionThrown_Failure()
        {
            const string Error = "Err";

            _postTagRepository.DeleteAsync(Arg.Any<long>(), Arg.Any<string>()).ThrowsAsync(new Exception(Error));

            var res = await _service.DeleteAsync(1, "a");

            Assert.False(res.Success);
            Assert.Equal(Error, res.Error);
        }
    }
}
