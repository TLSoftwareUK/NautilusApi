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
        
        Task<Guid> CreateSiteAsync(string name, string reference);

        Task<IEnumerable<SiteActivity>> GetSiteActivity(Guid id);
        
        Task SaveSiteAsync(Site site);

        void SetToken(string token);

        bool AuthSet();
    }
}
