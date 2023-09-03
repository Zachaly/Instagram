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
        private HttpClient _httpClient;

        public void AddAuthToken(string token)
        {
            if(_httpClient is not null)
            {
                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            }
        }

        public HttpClient Create()
        {
            if(_httpClient is null)
            {
                _httpClient = new HttpClient
                {
                    BaseAddress = new Uri(Configuration.ApiUrl)
                };
            }

            return _httpClient;
        }
    }
}
