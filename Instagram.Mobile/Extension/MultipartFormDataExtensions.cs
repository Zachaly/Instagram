using System.Net.Http.Headers;

namespace Instagram.Mobile.Extension
{
    public static class MultipartFormDataExtensions
    {
        public static void AddFileContent(this MultipartFormDataContent content, string filePath, string name)
        {
            var fileContent = new StreamContent(File.OpenRead(filePath));
            fileContent.Headers.ContentDisposition = new ContentDispositionHeaderValue("form-data")
            {
                FileName = filePath,
                Name = name
            };
            fileContent.Headers.ContentType = new MediaTypeHeaderValue("image/jpeg");
            content.Add(fileContent);
        }

        public static void AddFileContent(this MultipartFormDataContent content, IEnumerable<string> files, string name)
        {
            foreach (var filePath in files)
            {
                var fileContent = new StreamContent(File.OpenRead(filePath));
                fileContent.Headers.ContentDisposition = new ContentDispositionHeaderValue("form-data")
                {
                    FileName = filePath,
                    Name = name
                };
                fileContent.Headers.ContentType = new MediaTypeHeaderValue("image/jpeg");
                content.Add(fileContent);
            }
        }
    }
}
