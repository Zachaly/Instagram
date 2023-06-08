using Instagram.Domain.Entity;
using Instagram.Models.Post.Request;

namespace Instagram.Application.Abstraction
{
    public interface IPostFactory
    {
        Post Create(AddPostRequest request);
        IEnumerable<PostImage> CreateImages(IEnumerable<string> files, long postId);
    }
}
