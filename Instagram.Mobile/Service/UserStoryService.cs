using Instagram.Mobile.Extension;
using Instagram.Models.UserStory;
using Instagram.Models.UserStory.Request;

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

            request.AddFileContent(images, "Images");

            await _httpClient.PostAsync(Endpoint, request);
        }

        public Task<IEnumerable<UserStoryModel>> GetAsync(GetUserStoryRequest request)
            => _httpClient.GetWithRequestAsync<UserStoryModel, GetUserStoryRequest>(Endpoint, request);
    }
}
