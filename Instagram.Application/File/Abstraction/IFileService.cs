using Microsoft.AspNetCore.Http;

namespace Instagram.Application.Abstraction
{
    public interface IFileService
    {
        Task<string> SaveProfilePicture(IFormFile file);
        Task RemoveProfilePicture(string fileName);
        Task<FileStream> GetProfilePicture(string fileName);
    }
}
