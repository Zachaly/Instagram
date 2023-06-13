using Instagram.Models.Post;
using Instagram.Models.Post.Request;

namespace Instagram.Application.Abstraction
{
    public interface IPostService : IServiceBase<PostModel, GetPostRequest>
    {

    }
}
