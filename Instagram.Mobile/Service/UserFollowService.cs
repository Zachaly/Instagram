using Instagram.Models.UserFollow.Request;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;

namespace Instagram.Mobile.Service
{
    public interface IUserFollowService
    {
        Task<int> GetCountAsync(GetUserFollowRequest request);
    }

    public class UserFollowService : IUserFollowService
    {
        public const string Endpoint = "user-follow";
        private readonly HttpClient _httpClient;

        public UserFollowService(IHttpClientFactory httpClientFactory)
        {
            _httpClient = httpClientFactory.Create();
        }

        public async Task<int> GetCountAsync(GetUserFollowRequest request)
        {
            var response = await _httpClient.GetAsync(request.BuildQuery($"{Endpoint}/count"));

            return await response.Content.ReadFromJsonAsync<int>();
        }
    }
}
