using Instagram.Mobile.Extension;
using Instagram.Models.Notification;
using Instagram.Models.Notification.Request;
using System.Net.Http.Json;

namespace Instagram.Mobile.Service
{
    public interface INotificationService
    {
        Task<IEnumerable<NotificationModel>> GetAsync(GetNotificationRequest request);
        Task<int> GetCountAsync(GetNotificationRequest request);
        Task UpdateAsync(UpdateNotificationRequest request);
        Task DeleteByIdAsync(long id);
    }

    public class NotificationService : INotificationService
    {
        private readonly HttpClient _httpClient;
        private const string Endpoint = "notification";

        public NotificationService(IHttpClientFactory httpClientFactory)
        {
            _httpClient = httpClientFactory.Create();
        }

        public async Task DeleteByIdAsync(long id)
        {
            await _httpClient.DeleteAsync($"{Endpoint}/{id}");
        }

        public async Task<IEnumerable<NotificationModel>> GetAsync(GetNotificationRequest request)
            => await _httpClient.GetWithRequestAsync<NotificationModel, GetNotificationRequest>(Endpoint, request);

        public async Task<int> GetCountAsync(GetNotificationRequest request)
            => await _httpClient.GetCountAsync(Endpoint, request);

        public async Task UpdateAsync(UpdateNotificationRequest request)
        {
            await _httpClient.PatchAsJsonAsync(Endpoint, request);
        }
    }
}
