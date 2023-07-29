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
        private readonly string _relationImagePath;
        private readonly string _verificationDocumentPath;
        private readonly string _storyImagePath;

        public FileService(IConfiguration configuration)
        {
            _profilePicturePath = configuration["File:ProfilePicture"]!;
            _defaultFileName = configuration["File:Default"]!;
            _postImagePath = configuration["File:Post"]!;
            _relationImagePath = configuration["File:Relation"]!;
            _verificationDocumentPath = configuration["File:Verification"]!;
            _storyImagePath = configuration["File:Story"]!;
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

        public async Task<IEnumerable<string>> SavePostImagesAsync(IEnumerable<IFormFile> files)
        {
            Directory.CreateDirectory(_postImagePath);

            var names = new List<string>();

            foreach(var file in files)
            {
                var newName = $"{Guid.NewGuid()}.png";

                var path = Path.Combine(_postImagePath, newName);

                using (var stream = File.Create(path))
                {
                    await file.CopyToAsync(stream);
                }
                names.Add(newName);
            }

            return names;
        }

        public async Task<IEnumerable<string>> SaveRelationImagesAsync(IEnumerable<IFormFile> files)
        {
            Directory.CreateDirectory(_relationImagePath);

            var names = new List<string>();

            foreach (var file in files)
            {
                var newName = $"{Guid.NewGuid()}.png";

                var path = Path.Combine(_relationImagePath, newName);

                using (var stream = File.Create(path))
                {
                    await file.CopyToAsync(stream);
                }
                names.Add(newName);
            }

            return names;
        }

        public Task RemoveRelationImageAsync(string fileName)
        {
            File.Delete(Path.Join(_relationImagePath, fileName));

            return Task.CompletedTask;
        }

        public Task<FileStream> GetRelationImageAsync(string fileName)
            => Task.FromResult(ReadFile(_relationImagePath, fileName));

        public async Task<string> SaveRelationImageAsync(IFormFile file)
        {
            Directory.CreateDirectory(_relationImagePath);

            var newName = $"{Guid.NewGuid()}.png";

            var path = Path.Combine(_relationImagePath, newName);

            using (var stream = File.Create(path))
            {
                await file.CopyToAsync(stream);
            }

            return newName;
        }

        public async Task<string> SaveVerificationDocumentAsync(IFormFile file)
        {
            Directory.CreateDirectory(_verificationDocumentPath);

            var newName = $"{Guid.NewGuid()}.png";

            var path = Path.Combine(_verificationDocumentPath, newName);

            using (var stream = File.Create(path))
            {
                await file.CopyToAsync(stream);
            }

            return newName;
        }

        public Task RemoveVerificationDocumentAsync(string fileName)
        {
            File.Delete(Path.Join(_verificationDocumentPath, fileName));

            return Task.CompletedTask;
        }

        public Task<FileStream> GetVerificationDocumentAsync(string fileName)
            => Task.FromResult(ReadFile(_verificationDocumentPath, fileName));

        public async Task<IEnumerable<string>> SaveStoryImagesAsync(IEnumerable<IFormFile> files)
        {
            Directory.CreateDirectory(_storyImagePath);

            var names = new List<string>();

            foreach (var file in files)
            {
                var newName = $"{Guid.NewGuid()}.png";

                var path = Path.Combine(_storyImagePath, newName);

                using (var stream = File.Create(path))
                {
                    await file.CopyToAsync(stream);
                }
                names.Add(newName);
            }

            return names;
        }

        public Task RemoveStoryImageAsync(string fileName)
        {
            File.Delete(Path.Join(_storyImagePath, fileName));

            return Task.CompletedTask;
        }

        public Task<FileStream> GetStoryImageAsync(string fileName)
            => Task.FromResult(ReadFile(_storyImagePath, fileName));
    }
}
