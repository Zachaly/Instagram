using Microsoft.AspNetCore.Http;

namespace Instagram.Application.Abstraction
{
    public interface IFileService
    {
        Task<string> SaveProfilePictureAsync(IFormFile file);
        Task RemoveProfilePictureAsync(string fileName);
        Task<FileStream> GetProfilePictureAsync(string fileName);

        Task<IEnumerable<string>> SavePostImagesAsync(IEnumerable<IFormFile> files);
        Task RemovePostImageAsync(string fileName);
        Task<FileStream> GetPostImageAsync(string fileName);
    }
}
