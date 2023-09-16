using Instagram.Mobile.Extension;

namespace Instagram.Mobile.Service
{
    public interface IRelationImageService
    {
        Task AddAsync(long relationId, string filePath);
        Task DeleteAsync(long imageId);
    }

    public class RelationImageService : IRelationImageService
    {
        private readonly HttpClient _httpClient;
        private const string Endpoint = "relation-image";

        public RelationImageService(IHttpClientFactory httpClientFactory)
        {
            _httpClient = httpClientFactory.Create();
        }

        public async Task AddAsync(long relationId, string filePath)
        {
            var request = new MultipartFormDataContent
            {
                { new StringContent(relationId.ToString()), "RelationId" }
            };

            request.AddFileContent(filePath, "File");

            await _httpClient.PostAsync(Endpoint, request);
        }

        public async Task DeleteAsync(long imageId)
        {
            await _httpClient.DeleteAsync($"{Endpoint}/{imageId}");
        }
    }
}
