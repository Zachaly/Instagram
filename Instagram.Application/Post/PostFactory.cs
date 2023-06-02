using Instagram.Application.Abstraction;
using Instagram.Domain.Entity;
using Instagram.Models.Post.Request;

namespace Instagram.Application
{
    public class PostFactory : IPostFactory
    {
        public Post Create(AddPostRequest request, string fileName)
        {
            throw new NotImplementedException();
        }
    }
}
