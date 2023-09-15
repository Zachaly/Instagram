using Instagram.Models.Relation;
using Instagram.Models.Relation.Request;
using Instagram.Models.Response;
using Newtonsoft.Json;
using System.Net;
using System.Net.Http.Headers;
using System.Net.Http.Json;

namespace Instagram.Mobile.Service
{
    public interface IRelationService
    {
        Task<IEnumerable<RelationModel>> GetAsync(GetRelationRequest request);
        Task AddAsync(long userId, string name, IEnumerable<string> filesNames);
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

        public async Task<IEnumerable<RelationModel>> GetAsync(GetRelationRequest request)
        {
            var response = await _httpClient.GetAsync(request.BuildQuery(Endpoint));

            return await response.Content.ReadFromJsonAsync<IEnumerable<RelationModel>>();
        }

        public async Task AddAsync(long userId, string name, IEnumerable<string> filesNames)
        {
            var request = new MultipartFormDataContent
            {
                { new StringContent(userId.ToString()), "UserId" },
                { new StringContent(name), "Name" }
            };

            foreach (var image in filesNames)
            {
                var fileContent = new StreamContent(File.OpenRead(image));
                fileContent.Headers.ContentDisposition = new ContentDispositionHeaderValue("form-data")
                {
                    FileName = image,
                    Name = "Files"
                };
                fileContent.Headers.ContentType = new MediaTypeHeaderValue("image/jpeg");
                request.Add(fileContent);
            }

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
