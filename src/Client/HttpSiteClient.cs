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

        private readonly string _baseUrl;
        private readonly string _siteDesignerUrl;
        private SiteCache _cache;
        private SiteUpdateNotificationService _notificationService;

        public HttpSiteClient(ApiOptions options, IHttpClientFactory clientFactory, SiteCache cache, SiteUpdateNotificationService notificationService)
        {
            _clientFactory = clientFactory;
            _baseUrl = options.BaseUrl;
            _siteDesignerUrl = options.SiteDesignerUrl;
            _cache = cache;
            _notificationService = notificationService;
        }

        public async Task<Site> GetSiteAsync(Guid id)
        {
            Site? cachedSite = _cache[id];
            if (cachedSite != null)
            {
                return cachedSite;
            }

            return await GetRemoteSite(id);
        }

        public async Task<Site> ReloadSiteAsync(Guid id)
        {
            return await GetRemoteSite(id);
        }

        private async Task<Site> GetRemoteSite(Guid id)
        {
            var request = new HttpRequestMessage(HttpMethod.Get, $"{_baseUrl}/site/{id}");
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", NautilusApi.BearerToken);

            var client = _clientFactory.CreateClient();
            var response = await client.SendAsync(request);         
			
            Site site = await response.Content.ReadFromJsonAsync<Site>();

            if (site.Geo == null)
            {
                site.Geo = new GeotechnicalInformation();
            }

            if (site.Trees == null)
            {
                site.Trees = new List<Tree>();
            }

            _cache[id] = site;
            await _notificationService.Start();
            await _notificationService.OpenSite(id);

            return site;
        }

        public async Task<Guid> CreateSiteAsync(string name, string reference)
        {
            Site site = new Site()
            {
                Name = name,
                Reference = reference,
                Owner = "",
                Trees = new List<Tree>(),
                Geo = new GeotechnicalInformation()
            };
            
            var request = new HttpRequestMessage(HttpMethod.Post, $"{_baseUrl}/site");
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", NautilusApi.BearerToken);
            
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
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", NautilusApi.BearerToken);

            var client = _clientFactory.CreateClient();
            var response = await client.SendAsync(request);

            return await response.Content.ReadFromJsonAsync<IEnumerable<SiteActivity>>();
        }


        public async Task SaveSiteAsync(Site site)
        {
            var request = new HttpRequestMessage(HttpMethod.Put, $"{_baseUrl}/site/{site.Id}");
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", NautilusApi.BearerToken);
            
            request.Content = new StringContent(
                JsonSerializer.Serialize(site),
                Encoding.UTF8,
                "application/json");

            var client = _clientFactory.CreateClient();
            var response = await client.SendAsync(request);
            
            //TODO check for success
        }

        public async Task DeleteSiteAsync(Guid siteId)
        {
            var request = new HttpRequestMessage(HttpMethod.Delete, $"{_baseUrl}/site/{siteId}");
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", NautilusApi.BearerToken);
            
            var client = _clientFactory.CreateClient();
            var response = await client.SendAsync(request);
        }

        public string GetUrl()
        {
            return _siteDesignerUrl;
        }

        public string GetSiteUrl(Guid site)
        {
            return $"{GetUrl()}/site/{site}";
        }

        public async Task<IEnumerable<SiteDefinition>> GetSiteDefinitionsAsync()
        {
            var request = new HttpRequestMessage(HttpMethod.Get, $"{_baseUrl}/site");
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", NautilusApi.BearerToken);

            var client = _clientFactory.CreateClient();
            var response = await client.SendAsync(request);

            return await response.Content.ReadFromJsonAsync<IEnumerable<SiteDefinition>>();
        }
    }
}
