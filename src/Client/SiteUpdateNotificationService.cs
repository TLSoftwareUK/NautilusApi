using Microsoft.AspNetCore.SignalR.Client;
using System;
using System.Threading.Tasks;

namespace TLS.Nautilus.Api
{
    internal class SiteUpdateNotificationService : ISiteUpdateNotificationService
    {
        private readonly string _baseUrl;
        private HubConnection _connection;
        private SiteCache _cache;
        
        public SiteUpdateNotificationService(ApiOptions options, SiteCache cache)
        {
            _baseUrl = options.BaseUrl;
            _cache = cache;
            
            _connection = new HubConnectionBuilder()
                .WithUrl($"{_baseUrl}/updatenotificationhub")
                .WithAutomaticReconnect()
                .Build();

            _connection.On<Guid>("SiteUpdated", (message) =>
            {
                _cache.MarkDirty(message);
            });
        }

        public async Task Start()
        {
            if (_connection.State == HubConnectionState.Disconnected)
            {
                await _connection.StartAsync();
            }
        }

        public async Task OpenSite(Guid id)
        {
            await _connection.InvokeAsync("OpenSite", id);
        }
    }
}
