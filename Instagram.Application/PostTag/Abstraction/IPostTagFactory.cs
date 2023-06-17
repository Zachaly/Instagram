using Instagram.Domain.Entity;
using Instagram.Models.PostTag.Request;

namespace Instagram.Application.Abstraction
{
    public interface IPostTagFactory
    {
        IEnumerable<PostTag> CreateMany(AddPostTagRequest request);
        IEnumerable<PostTag> CreateMany(long postId, IEnumerable<string> tags);
    }
}
