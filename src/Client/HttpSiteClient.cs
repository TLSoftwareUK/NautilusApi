using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using TLS.Nautilus.Api.Shared;
using TLS.Nautilus.Api.Shared.DataStructures;

namespace TLS.Nautilus.Api
{
    internal class HttpSiteClient : ISiteClient, IFileClient
    {
        private readonly IHttpClientFactory _clientFactory;

        private readonly string _baseUrl;
        private readonly string _siteDesignerUrl;
        private SiteCache _cache;
        private ISiteUpdateNotificationService _notificationService;

        private bool _authEnabled;

        public HttpSiteClient(ApiOptions options, IHttpClientFactory clientFactory, SiteCache cache, ISiteUpdateNotificationService notificationService)
        {
            _clientFactory = clientFactory;
            _baseUrl = options.SiteServiceBaseUrl;
            _siteDesignerUrl = options.SiteDesignerUrl;
            _cache = cache;
            _notificationService = notificationService;
            _authEnabled = options.AuthEnabled;
        }

        public async Task<ISite> GetSiteAsync(Guid id)
        {
            Site? cachedSite = _cache[id];
            if (cachedSite != null)
            {
                return cachedSite;
            }

            return await GetRemoteSite(id);
        }

        public async Task CalculateSiteAsync(Guid id)
        {
            var request = new HttpRequestMessage(HttpMethod.Get, $"{_baseUrl}/site/{id}/calculate");
            
            if(_authEnabled)
                request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", NautilusApi.BearerToken);

            var client = _clientFactory.CreateClient();
            var response = await client.SendAsync(request);
        }

        public async Task<ISite> ReloadSiteAsync(Guid id, string? owner = null)
        {
            return await GetRemoteSite(id, owner);
        }

        private async Task<ISite> GetRemoteSite(Guid id, string? owner = null)
        {
            HttpRequestMessage request;

            if (owner != null)
            {
                request = new HttpRequestMessage(HttpMethod.Get, $"{_baseUrl}/site/{id}?owner={owner}");
            }
            else
            {
                request = new HttpRequestMessage(HttpMethod.Get, $"{_baseUrl}/site/{id}");
            }
                
            
            if(_authEnabled)
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

            if (site.Definitions == null)
            {
                site.Definitions = new List<PlotDefinition>();
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
                Geo = new GeotechnicalInformation(),
                Definitions = new List<PlotDefinition>()
            };
            
            var request = new HttpRequestMessage(HttpMethod.Post, $"{_baseUrl}/site");
            
            if(_authEnabled)
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
            
            if(_authEnabled)
                request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", NautilusApi.BearerToken);

            var client = _clientFactory.CreateClient();
            var response = await client.SendAsync(request);

            return await response.Content.ReadFromJsonAsync<IEnumerable<SiteActivity>>();
        }


        public async Task SaveSiteAsync(ISite site, string? owner = null)
        {
            HttpRequestMessage request;

            if (owner != null)
            {
                request = new HttpRequestMessage(HttpMethod.Put, $"{_baseUrl}/site/{site.Id}?owner={owner}");
            }
            else
            {
                request = new HttpRequestMessage(HttpMethod.Put, $"{_baseUrl}/site/{site.Id}");
            }

            if (_authEnabled)
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
            
            if(_authEnabled)
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
            
            if(_authEnabled)
                request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", NautilusApi.BearerToken);

            var client = _clientFactory.CreateClient();
            var response = await client.SendAsync(request);

            return await response.Content.ReadFromJsonAsync<IEnumerable<SiteDefinition>>();
        }

        public async Task<Guid> UploadFile(Stream content, string filename)
        {
            var request = new HttpRequestMessage(HttpMethod.Post, $"{_baseUrl}/file");

            /*request.Content = new StringContent(
                JsonSerializer.Serialize(site),
                Encoding.UTF8,
                "application/json");*/

            MultipartFormDataContent upload = new MultipartFormDataContent();

            var file = new StreamContent(content);
            //file.Headers.ContentType = MediaTypeHeaderValue.Parse()
            upload.Add(file, "files", filename);
            request.Content = upload;            

            var client = _clientFactory.CreateClient();
            var response = await client.SendAsync(request);
            var responseString = (await response.Content.ReadAsStringAsync()).Replace("\"", "");
            return Guid.Parse(responseString);
        }

        public async Task RequestExportSiteAsync(Guid id)
        {
            var request = new HttpRequestMessage(HttpMethod.Get, $"{_baseUrl}/site/{id}/export");

            if (_authEnabled)
                request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", NautilusApi.BearerToken);

            var client = _clientFactory.CreateClient();
            var response = await client.SendAsync(request);

            if (response.IsSuccessStatusCode)
                return;

            //TODO: Handle errors
        }        
    }
}
