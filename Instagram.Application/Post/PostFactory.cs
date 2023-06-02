using Instagram.Application.Abstraction;
using Instagram.Domain.Entity;
using Instagram.Models.Post.Request;

namespace Instagram.Application
{
    public class PostFactory : IPostFactory
    {
        public Post Create(AddPostRequest reqeuest, string fileName)
        {
            throw new NotImplementedException();
        }
    }
}
