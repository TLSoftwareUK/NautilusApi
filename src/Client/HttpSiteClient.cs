using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using TLS.Nautilus.Api.Shared.DataStructures;

namespace TLS.Nautilus.Api
{
    internal class HttpSiteClient : ISiteClient
    {
        private readonly IHttpClientFactory _clientFactory;
        private string _bearerToken;

        private readonly string _baseUrl;

        public HttpSiteClient(ApiOptions options, IHttpClientFactory clientFactory)
        {
            _clientFactory = clientFactory;
            _baseUrl = options.BaseUrl;
        }

        public async Task<Site> GetSiteAsync(Guid id)
        {
            var request = new HttpRequestMessage(HttpMethod.Get, $"{_baseUrl}/site/{id}");
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", _bearerToken);

            var client = _clientFactory.CreateClient();
            var response = await client.SendAsync(request);

            return await response.Content.ReadFromJsonAsync<Site>();
        }

        public async Task<Guid> CreateSiteAsync(string name, string reference)
        {
            Site site = new Site()
            {
                Name = name,
                Reference = reference,
                Owner = ""
            };
            
            var request = new HttpRequestMessage(HttpMethod.Post, $"{_baseUrl}/site");
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", _bearerToken);
            
            request.Content = new StringContent(
                JsonSerializer.Serialize(site),
                Encoding.UTF8,
                "application/json");

            var client = _clientFactory.CreateClient();
            var response = await client.SendAsync(request);
            
            return await response.Content.ReadFromJsonAsync<Guid>();
        }

        public async Task<IEnumerable<SiteActivity>> GetSiteActivity(Guid id)
        {
            var request = new HttpRequestMessage(HttpMethod.Get, $"{_baseUrl}/site/{id}/activity");
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", _bearerToken);

            var client = _clientFactory.CreateClient();
            var response = await client.SendAsync(request);

            return await response.Content.ReadFromJsonAsync<IEnumerable<SiteActivity>>();
        }


        public async Task SaveSiteAsync(Site site)
        {
            var request = new HttpRequestMessage(HttpMethod.Put, $"{_baseUrl}/site/{site.Id}");
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", _bearerToken);
            
            request.Content = new StringContent(
                JsonSerializer.Serialize(site),
                Encoding.UTF8,
                "application/json");

            var client = _clientFactory.CreateClient();
            var response = await client.SendAsync(request);
            
            //TODO check for success
        }

        public void SetToken(string token)
        {
            _bearerToken = token;
        }

        public async Task<IEnumerable<SiteDefinition>> GetSiteDefinitionsAsync()
        {
            var request = new HttpRequestMessage(HttpMethod.Get, $"{_baseUrl}/site");
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", _bearerToken);

            var client = _clientFactory.CreateClient();
            var response = await client.SendAsync(request);

            return await response.Content.ReadFromJsonAsync<IEnumerable<SiteDefinition>>();
        }

        public bool AuthSet()
        {
            return !string.IsNullOrWhiteSpace(_bearerToken);
        }
    }
}
