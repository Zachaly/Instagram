using Instagram.Application.Abstraction;
using Instagram.Domain.Entity;
using Instagram.Models.PostTag.Request;

namespace Instagram.Application
{
    public class PostTagFactory : IPostTagFactory
    {
        public IEnumerable<PostTag> CreateMany(AddPostTagRequest request)
            => request.Tags.Select(name => new PostTag { Tag = name.ToLower(), PostId = request.PostId });

        public IEnumerable<PostTag> CreateMany(long postId, IEnumerable<string> tags)
            => tags.Select(name => new PostTag { PostId = postId, Tag = name.ToLower() });
    }
}
