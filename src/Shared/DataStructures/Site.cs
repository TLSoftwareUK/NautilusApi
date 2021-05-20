using System;
using System.Collections.Generic;

namespace TLS.Nautilus.Api.Shared.DataStructures
{
    public class Site
    {
        public event EventHandler? OnSiteChanged;
        
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Reference { get; set; }
        public string Owner { get; set; }
		
		public GeotechnicalInformation Geo { get; set; }
        
        public List<Tree> Trees { get; set; }

        public void SiteChanged()
        {
            OnSiteChanged?.Invoke(this, null);
        }
    }
}
