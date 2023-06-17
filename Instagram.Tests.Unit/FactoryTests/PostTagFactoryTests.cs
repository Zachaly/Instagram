using Instagram.Application;
using Instagram.Models.PostTag.Request;

namespace Instagram.Tests.Unit.FactoryTests
{
    public class PostTagFactoryTests
    {
        private readonly PostTagFactory _factory;

        public PostTagFactoryTests()
        {
            _factory = new PostTagFactory();
        }

        [Fact]
        public void CreateMany_WithRequest_CreatesValidEntities()
        {
            var request = new AddPostTagRequest
            {
                PostId = 1,
                Tags = new string[] { "tag1", "TAG2", "tagGG" }
            };

            var tags = _factory.CreateMany(request);

            Assert.Equivalent(request.Tags.Select(tag => tag.ToLower()), tags.Select(x => x.Tag));
            Assert.All(tags, tag => Assert.Equal(request.PostId, tag.PostId));
        }

        [Fact]
        public void CreateMany_WithPostIdAndTags_CreatesValidEntities()
        {
            const long PostId = 1;
            var names = new string[] { "tag1", "TAG2", "tagGG" };

            var tags = _factory.CreateMany(PostId, names);

            Assert.Equivalent(names.Select(tag => tag.ToLower()), tags.Select(x => x.Tag));
            Assert.All(tags, tag => Assert.Equal(PostId, tag.PostId));
        }
    }
}
