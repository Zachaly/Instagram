using Microsoft.AspNetCore.SignalR.Client;
using System.Net.Http.Headers;

namespace Instagram.Mobile.Service
{
    public interface IWebsocketService
    {
        Task StartConnection(string endpoint);
        Task StopConnection();
        void AddListener<T>(string action, Action<T> listener);
        void AddListener<T, T2>(string action, Action<T, T2> listener);
    }

    public class WebsocketService : IWebsocketService
    {
        private HubConnection _connection;
        private readonly IAuthorizationService _authorizationService;

        public WebsocketService(IAuthorizationService authorizationService)
        {
            _authorizationService = authorizationService;
        }

        public async Task StartConnection(string endpoint)
        {
            var builder = new HubConnectionBuilder()
                .WithUrl($"{Configuration.WebsocketUrl}/{endpoint}", opt =>
                {
                    opt.Headers.Add("Authorization", new AuthenticationHeaderValue("Bearer",
                        _authorizationService.UserData.AuthToken).ToString());
                });

            _connection = builder.Build();

            await _connection.StartAsync();
        }

        public async Task StopConnection()
        {
            await _connection.StopAsync();
        }

        public void AddListener<T>(string action, Action<T> listener)
        {
            _connection.On(action, listener);
        }

        public void AddListener<T, T2>(string action, Action<T, T2> listener)
        {
            _connection.On(action, listener);
        }
    }
}
