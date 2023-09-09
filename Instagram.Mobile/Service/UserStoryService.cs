using Instagram.Models.UserStory;
using Instagram.Models.UserStory.Request;
using System.Net.Http.Headers;
using System.Net.Http.Json;

namespace Instagram.Mobile.Service
{
    public interface IUserStoryService
    {
        Task<IEnumerable<UserStoryModel>> GetAsync(GetUserStoryRequest request);
        Task AddAsync(IEnumerable<string> images, long userId);
    }

    public class UserStoryService : IUserStoryService
    {
        const string Endpoint = "user-story";
        private readonly HttpClient _httpClient;

        public UserStoryService(IHttpClientFactory httpClientFactory)
        {
            _httpClient = httpClientFactory.Create();
        }

        public async Task AddAsync(IEnumerable<string> images, long userId)
        {
            var request = new MultipartFormDataContent
            {
                { new StringContent(userId.ToString()), "UserId" }
            };

            foreach (var image in images)
            {
                var fileContent = new StreamContent(File.OpenRead(image));
                fileContent.Headers.ContentDisposition = new ContentDispositionHeaderValue("form-data")
                {
                    FileName = image,
                    Name = "Images"
                };
                fileContent.Headers.ContentType = new MediaTypeHeaderValue("image/jpeg");
                request.Add(fileContent);
            }

            await _httpClient.PostAsync(Endpoint, request);
        }

        public async Task<IEnumerable<UserStoryModel>> GetAsync(GetUserStoryRequest request)
        {
            var response = await _httpClient.GetAsync(request.BuildQuery(Endpoint));

            return await response.Content.ReadFromJsonAsync<IEnumerable<UserStoryModel>>();
        }
    }
}
