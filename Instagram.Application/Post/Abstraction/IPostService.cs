using Instagram.Models.Post;
using Instagram.Models.Post.Request;

namespace Instagram.Application.Abstraction
{
    public interface IPostService
    {
        Task<IEnumerable<PostModel>> GetAsync(GetPostRequest request);
        Task<PostModel> GetByIdAsync(long id);
        Task<int> GetCountAsync(GetPostRequest request);
    }
}
