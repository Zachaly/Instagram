using Instagram.Application.Abstraction;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;

namespace Instagram.Application
{
    public class FileService : IFileService
    {
        private readonly string _profilePicturePath;
        private readonly string _defaultFileName;
        private readonly string _postImagePath;

        public FileService(IConfiguration configuration)
        {
            _profilePicturePath = configuration["File:ProfilePicture"]!;
            _defaultFileName = configuration["File:Default"]!;
            _postImagePath = configuration["File:Post"]!;
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

        public async Task<string> SavePostImageAsync(IFormFile file)
        {
            Directory.CreateDirectory(_postImagePath);

            var newName = $"{Guid.NewGuid()}.png";

            var path = Path.Combine(_postImagePath, newName);

            using (var stream = File.Create(path))
            {
                await file.CopyToAsync(stream);
            }

            return newName;
        }

        public Task RemovePostImageAsync(string fileName)
        {
            File.Delete(Path.Join(_postImagePath, fileName));

            return Task.CompletedTask;
        }

        public Task<FileStream> GetPostImageAsync(string fileName)
            => Task.FromResult(ReadFile(_postImagePath, fileName));

        public Task<IEnumerable<string>> SavePostImagesAsync(IEnumerable<IFormFile> files)
        {
            throw new NotImplementedException();
        }
    }
}
