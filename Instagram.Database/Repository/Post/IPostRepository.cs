using Instagram.Database.Repository.Abstraction;
using Instagram.Domain.Entity;
using Instagram.Models.Post;
using Instagram.Models.Post.Request;

namespace Instagram.Database.Repository
{
    public interface IPostRepository : IRepositoryBase<Post, PostModel, GetPostRequest>
    {
    }
}
