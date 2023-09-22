using Instagram.Mobile.Extension;
using Instagram.Models.DirectMessage;
using Instagram.Models.DirectMessage.Request;
using Instagram.Models.Response;
using Newtonsoft.Json;
using System.Net;
using System.Net.Http.Json;

namespace Instagram.Mobile.Service
{
    public interface IDirectMessageService
    {
        Task<long> AddAsync(AddDirectMessageRequest request);
        Task<IEnumerable<DirectMessageModel>> GetAsync(GetDirectMessageRequest request);
        Task UpdateAsync(UpdateDirectMessageRequest request);
    }

    public class DirectMessageService : IDirectMessageService
    {
        private readonly HttpClient _httpClient;
        private const string Endpoint = "direct-message";

        public DirectMessageService(IHttpClientFactory httpClientFactory)
        {
            _httpClient = httpClientFactory.Create();
        }

        public async Task<long> AddAsync(AddDirectMessageRequest request)
        {
            var response = await _httpClient.PostAsJsonAsync(Endpoint, request);

            if(response.StatusCode == HttpStatusCode.BadRequest)
            {
                var responseContent = await response.Content.ReadAsStringAsync();
                throw new InvalidRequestException(JsonConvert.DeserializeObject<ResponseModel>(responseContent));
            }

            var content = await response.Content.ReadFromJsonAsync<ResponseModel>();

            return content.NewEntityId.Value;
        }

        public Task<IEnumerable<DirectMessageModel>> GetAsync(GetDirectMessageRequest request)
            => _httpClient.GetWithRequestAsync<DirectMessageModel, GetDirectMessageRequest>(Endpoint, request);

        public async Task UpdateAsync(UpdateDirectMessageRequest request)
        {
            await _httpClient.PatchAsJsonAsync(Endpoint, request);
        }
    }
}
