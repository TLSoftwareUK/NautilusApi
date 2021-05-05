using System;

namespace TLS.Nautilus.Api.Shared.DataStructures
{
    public class SiteActivity
    {
        public string Description { get; set; }
        public string ActionBy { get; set; }
        
        public DateTime Time { get; set; }
    }
}
