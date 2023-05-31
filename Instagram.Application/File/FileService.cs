using Instagram.Application.Abstraction;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;

namespace Instagram.Application
{
    public class FileService : IFileService
    {
        private string _profilePicturePath;
        private string _defaultFileName;

        public FileService(IConfiguration configuration)
        {
            _profilePicturePath = configuration["File:ProfilePicture"]!;
            _defaultFileName = configuration["File:Default"]!;
        }

        public Task<FileStream> GetProfilePictureAsync(string fileName)
        {
            throw new NotImplementedException();
        }

        public Task RemoveProfilePictureAsync(string fileName)
        {
            throw new NotImplementedException();
        }

        public Task<string> SaveProfilePictureAsync(IFormFile file)
        {
            throw new NotImplementedException();
        }
    }
}
