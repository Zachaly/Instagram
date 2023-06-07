using Instagram.Database.Repository.Abstraction;
using Instagram.Domain.Entity;
using Instagram.Models.PostImage;
using Instagram.Models.PostImage.Request;

namespace Instagram.Database.Repository
{
    public interface IPostImageRepository : IRepositoryBase<PostImage, PostImageModel, GetPostImageRequest>
    {
    }
}
