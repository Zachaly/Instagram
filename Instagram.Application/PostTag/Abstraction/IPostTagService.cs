using Instagram.Models.PostTag;
using Instagram.Models.PostTag.Request;
using Instagram.Models.Response;

namespace Instagram.Application.Abstraction
{
    public interface IPostTagService : IKeylessServiceBase<PostTagModel, GetPostTagRequest, AddPostTagRequest>
    {
        Task<ResponseModel> DeleteAsync(long postId, string tag);
    }
}
