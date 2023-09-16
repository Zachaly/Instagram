using Instagram.Models;
using System.Net;
using System.Net.Http.Json;

namespace Instagram.Mobile.Extension
{
    public static class HttpClientExtensions
    {
        public static async Task<IEnumerable<T>> GetWithRequestAsync<T, TRequest>(this HttpClient httpClient, string endpoint, TRequest request)
            where TRequest : PagedRequest
        {
            var response = await httpClient.GetAsync(request.BuildQuery(endpoint));

            return await response.Content.ReadFromJsonAsync<IEnumerable<T>>();
        }

        public static async Task<T> GetByIdAsync<T>(this HttpClient httpClient, string endpoint, long id)
        {
            var response = await httpClient.GetAsync($"{endpoint}/{id}");

            if(response.StatusCode == HttpStatusCode.NotFound)
            {
                throw new NotFoundException(typeof(T).Name.Replace("Model", ""));
            }

            return await response.Content.ReadFromJsonAsync<T>();
        }

        public static async Task<int> GetCountAsync<TRequest>(this HttpClient httpClient, string endpoint, TRequest request)
            where TRequest : PagedRequest
        {
            var response = await httpClient.GetAsync(request.BuildQuery($"{endpoint}/count"));

            return await response.Content.ReadFromJsonAsync<int>();
        }
    }
}
