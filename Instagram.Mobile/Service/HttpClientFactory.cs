using System.Net.Http.Headers;

namespace Instagram.Mobile.Service
{
    public interface IHttpClientFactory
    {
        HttpClient Create();
        void AddAuthToken(string token);
    }

    public class HttpClientFactory : IHttpClientFactory
    {
        private HttpClient _httpClient = null;

        public void AddAuthToken(string token)
        {
            if(_httpClient is not null)
            {
                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer ", token);
            }
        }

        public HttpClient Create()
        {
            if(_httpClient is not null)
            {
                return _httpClient;
            }

            return new HttpClient
            {
                BaseAddress = new Uri(Configuration.ApiUrl)
            };
        }
    }
}
