using Instagram.Domain.Entity;
using Instagram.Models.Post.Request;

namespace Instagram.Application.Abstraction
{
    public interface IPostFactory : IEntityFactory<Post, AddPostRequest>
    {
        IEnumerable<PostImage> CreateImages(IEnumerable<string> files, long postId);
    }
}
