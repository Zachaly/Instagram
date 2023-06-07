using Instagram.Application.Abstraction;
using Instagram.Domain.Entity;
using Instagram.Models.Post.Request;

namespace Instagram.Application
{
    public class PostFactory : IPostFactory
    {
        public Post Create(AddPostRequest request)
        => new Post
        {
            Content = request.Content,
            CreatorId = request.CreatorId,
            Created = DateTimeOffset.Now.ToUnixTimeSeconds()
        };

        public IEnumerable<PostImage> CreateImages(IEnumerable<string> files, long postId)
            => files.Select(file => new PostImage
            {
                File = file,
                PostId = postId
            });
    }
}
