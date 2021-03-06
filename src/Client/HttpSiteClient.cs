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
    internal class HttpSiteClient : ISiteClient, IFileClient, IProfileClient
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

        public async Task GenerateSiteAsync(Guid id)
        {
            var request = new HttpRequestMessage(HttpMethod.Get, $"{_baseUrl}/site/{id}/generate");

            if (_authEnabled)
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

            if (_authEnabled)
            {                
                request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", NautilusApi.BearerToken);
            }

            var client = _clientFactory.CreateClient();
            var response = await client.SendAsync(request);

            if (!response.IsSuccessStatusCode)
                throw new InvalidOperationException($"GetRemoteSite failed with http status code {response.StatusCode}");
			
            Site? site = await response.Content.ReadFromJsonAsync<Site>();

            if (site == null)
                throw new InvalidOperationException($"GetRemoteSite failed with empty site");

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
            try
            {
                await _notificationService.Start();
                await _notificationService.OpenSite(id);
            } catch (Exception e)
            {
                int i = 0;
            }

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

            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadFromJsonAsync<IEnumerable<SiteDefinition>>();
            } else
            {
                throw new InvalidOperationException($"Invalid response to get site definitions, {response.StatusCode} {response.ReasonPhrase}");
            }
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

        public Task DiscardSiteChangesAsync(ISite site, string? owner = null)
        {
            throw new NotImplementedException();
        }

        public async Task<ISite> AddDrawingAsync(Guid id, string name, Stream drawing, DrawingType type, string? owner = null)
        {
            HttpRequestMessage request;
            if (owner != null)
            {
                request = new HttpRequestMessage(HttpMethod.Put, $"{_baseUrl}/site/{id}/drawings?owner={owner}&type={type}");
            }
            else
            {
                request = new HttpRequestMessage(HttpMethod.Put, $"{_baseUrl}/site/{id}/drawings?type={type}");
            }
            
            MultipartFormDataContent upload = new MultipartFormDataContent();

            if (_authEnabled)
                request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", NautilusApi.BearerToken);

            var file = new StreamContent(drawing);
            //file.Headers.ContentType = MediaTypeHeaderValue.Parse()
            upload.Add(file, "files", name);
            request.Content = upload;

            var client = _clientFactory.CreateClient();
            var response = await client.SendAsync(request);              
            
            //TODO: Add error handling

            return await ReloadSiteAsync(id, owner);

        }

        public Task<Stream> GetDrawingAsync(Guid id, string name, DrawingType type, string? owner = null)
        {
            throw new NotImplementedException();
        }

        public async Task<IProfile?> GetProfileAsync()
        {
            HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Get, $"{_baseUrl}/profile");
                        
            if (_authEnabled)
            {
                request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", NautilusApi.BearerToken);
            }

            var client = _clientFactory.CreateClient();
            var response = await client.SendAsync(request);

            if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
                return null;

            if (!response.IsSuccessStatusCode)
                throw new InvalidOperationException($"GetProfileAsync failed with http status code {response.StatusCode}");

            Profile? profile = await response.Content.ReadFromJsonAsync<Profile>();

            if (profile == null)
                throw new InvalidOperationException($"GetProfileAsync failed with empty or malformed profile");
            
            return profile;
        }

        public async Task UpdateProfileAsync(IProfile profile)
        {
            HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Put, $"{_baseUrl}/profile");

            if (_authEnabled)
            {
                request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", NautilusApi.BearerToken);
            }

            Profile body = profile as Profile;
                        
            request.Content = new StringContent(
                JsonSerializer.Serialize(body), Encoding.UTF8, "application/json");

            var client = _clientFactory.CreateClient();
            var response = await client.SendAsync(request);
            
            //TODO: Error handling
        }

        public async Task CreateProfileAsync(IProfile profile)
        {
            HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Post, $"{_baseUrl}/profile");

            if (_authEnabled)
            {
                request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", NautilusApi.BearerToken);
            }

            Profile body = profile as Profile;

            request.Content = new StringContent(
                JsonSerializer.Serialize(body), Encoding.UTF8, "application/json");

            var client = _clientFactory.CreateClient();
            var response = await client.SendAsync(request);

            //TODO: Error handling
        }
    }
}
