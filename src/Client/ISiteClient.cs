using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TLS.Nautilus.Api.Shared.DataStructures;

namespace TLS.Nautilus.Api
{
    public interface ISiteClient
    {
        Task<IEnumerable<SiteDefinition>> GetSiteDefinitionsAsync();
        
        Task<Site> GetSiteAsync(Guid id);
        
        Task CalculateSiteAsync(Guid id);
        
        Task<Site> ReloadSiteAsync(Guid id, string? owner = null);
        
        Task<Guid> CreateSiteAsync(string name, string reference);

        Task<IEnumerable<SiteActivity>> GetSiteActivity(Guid id);
        
        Task SaveSiteAsync(Site site, string? owner = null);
        
        Task DeleteSiteAsync(Guid siteId);

        string GetUrl();
        
        string GetSiteUrl(Guid id);
    }
}
