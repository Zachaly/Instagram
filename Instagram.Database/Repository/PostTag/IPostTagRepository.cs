using Instagram.Database.Repository.Abstraction;
using Instagram.Domain.Entity;
using Instagram.Models.PostTag;
using Instagram.Models.PostTag.Request;

namespace Instagram.Database.Repository
{
    public interface IPostTagRepository : IKeylessRepositoryBase<PostTag, PostTagModel, GetPostTagRequest>
    {
        Task DeleteAsync(long postId, string tag);
        Task DeleteByPostIdAsync(long postId);
    }
}
