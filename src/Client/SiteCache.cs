using System;
using System.Collections.Generic;
using TLS.Nautilus.Api.Shared.DataStructures;

namespace TLS.Nautilus.Api
{
    internal class SiteCache
    {
        private Dictionary<Guid, SiteCacheEntry> _cache;

        public SiteCache()
        {
            _cache = new Dictionary<Guid, SiteCacheEntry>();
        }

        public Site? this[Guid siteId]
        {
            get
            {
                if (_cache.ContainsKey(siteId))
                    return _cache[siteId].Site;

                return null;
            }
            set
            {
                if (_cache.ContainsKey(siteId))
                    _cache.Remove(siteId);
                
                _cache.Add(siteId, new SiteCacheEntry()
                {
                    Dirty = false,
                    Site = value
                });
            }
        }

        public void MarkDirty(Guid siteId)
        {
            _cache[siteId].Dirty = true;
            _cache[siteId].Site.SiteChanged();
        }
    }

    public class SiteCacheEntry
    {
        public Site Site { get; set; }
        public bool Dirty { get; set; }
    }
}
