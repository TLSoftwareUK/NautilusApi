using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using TLS.Nautilus.Api.Shared;
using TLS.Nautilus.Api.Shared.DataStructures;

namespace TLS.Nautilus.Api
{
    public interface ISiteClient
    {
        Task<IEnumerable<SiteDefinition>> GetSiteDefinitionsAsync();

        Task<ISite> GetSiteAsync(Guid id);

        Task CalculateSiteAsync(Guid id);

        Task GenerateSiteAsync(Guid id);

        Task RequestExportSiteAsync(Guid id);

        Task<ISite> ReloadSiteAsync(Guid id, string? owner = null);

        Task<Guid> CreateSiteAsync(string name, string reference);

        Task<IEnumerable<SiteActivity>> GetSiteActivity(Guid id);

        Task SaveSiteAsync(ISite site, string? owner = null);

        Task DiscardSiteChangesAsync(ISite site, string? owner = null);

        Task DeleteSiteAsync(Guid siteId);

        string GetUrl();

        string GetSiteUrl(Guid id);

        Task AddDrawingAsync(Guid id, string name, Stream drawing, string? owner = null);

        Task<Stream> GetDrawingAsync(Guid id, string name, string? owner = null);
    }
}
