using Instagram.Models.UserStory;
using Instagram.Models.UserStory.Request;
using System.Net.Http.Json;

namespace Instagram.Mobile.Service
{
    public interface IUserStoryService
    {
        Task<IEnumerable<UserStoryModel>> GetAsync(GetUserStoryRequest request);
    }

    public class UserStoryService : IUserStoryService
    {
        const string Endpoint = "user-story";
        private readonly HttpClient _httpClient;

        public UserStoryService(IHttpClientFactory httpClientFactory)
        {
            _httpClient = httpClientFactory.Create();
        }

        public async Task<IEnumerable<UserStoryModel>> GetAsync(GetUserStoryRequest request)
        {
            var response = await _httpClient.GetAsync(request.BuildQuery(Endpoint));

            return await response.Content.ReadFromJsonAsync<IEnumerable<UserStoryModel>>();
        }
    }
}
