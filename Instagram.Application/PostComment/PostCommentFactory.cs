using Instagram.Application.Abstraction;
using Instagram.Domain.Entity;
using Instagram.Models.PostComment.Request;

namespace Instagram.Application
{
    public class PostCommentFactory : IPostCommentFactory
    {
        public PostComment Create(AddPostCommentRequest request)
        {
            throw new NotImplementedException();
        }
    }
}
