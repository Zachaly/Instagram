using Instagram.Models.PostTag;
using Instagram.Models.PostTag.Request;
using Instagram.Models.Response;

namespace Instagram.Application.Abstraction
{
    public interface IPostTagService : IKeylessServiceBase<PostTagModel, GetPostTagRequest>
    {
        Task<ResponseModel> AddAsync(AddPostTagRequest request);
        Task<ResponseModel> DeleteAsync(long postId, string tag);
    }
}
