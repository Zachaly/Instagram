using Instagram.Application.Abstraction;
using Instagram.Domain.Entity;
using Instagram.Models.PostTag.Request;

namespace Instagram.Application
{
    public class PostTagFactory : IPostTagFactory
    {
        public IEnumerable<PostTag> CreateMany(AddPostTagRequest request)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<PostTag> CreateMany(long postId, IEnumerable<string> tags)
        {
            throw new NotImplementedException();
        }
    }
}
