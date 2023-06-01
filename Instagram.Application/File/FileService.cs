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

        private FileStream ReadFile(string path, string name)
            => File.OpenRead(Path.Join(path, name));

        public Task<FileStream> GetProfilePictureAsync(string fileName)
        {
            if(string.IsNullOrWhiteSpace(fileName))
            {
                return Task.FromResult(ReadFile(_profilePicturePath, _defaultFileName));
            }

            return Task.FromResult(ReadFile(_profilePicturePath, fileName));
        }

        public Task RemoveProfilePictureAsync(string fileName)
        {
            if(string.IsNullOrWhiteSpace(fileName) || fileName == _defaultFileName) 
            {
                return Task.CompletedTask;
            }

            File.Delete(Path.Join(_profilePicturePath, fileName));

            return Task.CompletedTask;
        }

        public async Task<string> SaveProfilePictureAsync(IFormFile file)
        {
            if(file is null)
            {
                return _defaultFileName;
            }

            Directory.CreateDirectory(_profilePicturePath);

            var newName = $"{Guid.NewGuid()}.png";

            var path = Path.Combine(_profilePicturePath, newName);

            using(var stream = File.Create(path))
            {
                await file.CopyToAsync(stream);
            }

            return newName;
        }
    }
}
