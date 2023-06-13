using Instagram.Application.Abstraction;
using Instagram.Database.Repository;
using Instagram.Domain.Entity;
using Instagram.Models.Post;
using Instagram.Models.Post.Request;

namespace Instagram.Application
{
    public class PostService : ServiceBase<Post, PostModel, GetPostRequest, IPostRepository>, IPostService
    {
        public PostService(IPostRepository postRepository) : base(postRepository) { }
    }
}
