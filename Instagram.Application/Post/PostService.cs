using Instagram.Application.Abstraction;
using Instagram.Database.Repository;
using Instagram.Models.Post;
using Instagram.Models.Post.Request;

namespace Instagram.Application
{
    public class PostService : IPostService
    {
        private readonly IPostRepository _postRepository;

        public PostService(IPostRepository postRepository)
        {
            _postRepository = postRepository;
        }

        public Task<IEnumerable<PostModel>> GetAsync(GetPostRequest request)
        {
            throw new NotImplementedException();
        }

        public Task<PostModel> GetByIdAsync(long id)
        {
            throw new NotImplementedException();
        }
    }
}
