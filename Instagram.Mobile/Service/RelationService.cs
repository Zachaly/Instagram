using Instagram.Models.Relation;
using Instagram.Models.Relation.Request;
using System.Net.Http.Json;

namespace Instagram.Mobile.Service
{
    public interface IRelationService
    {
        Task<IEnumerable<RelationModel>> GetAsync(GetRelationRequest request);
    }

    public class RelationService : IRelationService
    {
        private readonly HttpClient _httpClient;
        private const string Endpoint = "relation";

        public RelationService(IHttpClientFactory httpClientFactory)
        {
            _httpClient = httpClientFactory.Create();
        }

        public async Task<IEnumerable<RelationModel>> GetAsync(GetRelationRequest request)
        {
            var response = await _httpClient.GetAsync(request.BuildQuery(Endpoint));

            return await response.Content.ReadFromJsonAsync<IEnumerable<RelationModel>>();
        }
    }
}
