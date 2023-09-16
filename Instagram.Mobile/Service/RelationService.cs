using Instagram.Mobile.Extension;
using Instagram.Models.Relation;
using Instagram.Models.Relation.Request;
using Instagram.Models.Response;
using Newtonsoft.Json;
using System.Net;

namespace Instagram.Mobile.Service
{
    public interface IRelationService
    {
        Task<IEnumerable<RelationModel>> GetAsync(GetRelationRequest request);
        Task AddAsync(long userId, string name, IEnumerable<string> fileNames);
        Task DeleteByIdAsync(long id);
    }

    public class RelationService : IRelationService
    {
        private readonly HttpClient _httpClient;
        private const string Endpoint = "relation";

        public RelationService(IHttpClientFactory httpClientFactory)
        {
            _httpClient = httpClientFactory.Create();
        }

        public Task<IEnumerable<RelationModel>> GetAsync(GetRelationRequest request)
            => _httpClient.GetWithRequestAsync<RelationModel, GetRelationRequest>(Endpoint, request);

        public async Task AddAsync(long userId, string name, IEnumerable<string> fileNames)
        {
            var request = new MultipartFormDataContent
            {
                { new StringContent(userId.ToString()), "UserId" },
                { new StringContent(name), "Name" }
            };

            request.AddFileContent(fileNames, "Files");

            var response = await _httpClient.PostAsync(Endpoint, request);

            if(response.StatusCode == HttpStatusCode.BadRequest)
            {
                throw new InvalidRequestException(JsonConvert.DeserializeObject<ResponseModel>(await response.Content.ReadAsStringAsync()));
            }
        }

        public async Task DeleteByIdAsync(long id)
        {
            await _httpClient.DeleteAsync($"{Endpoint}/{id}");
        }
    }
}
