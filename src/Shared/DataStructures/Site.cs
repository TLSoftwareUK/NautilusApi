using System;
using System.Collections.Generic;

namespace TLS.Nautilus.Api.Shared.DataStructures
{
    public class Site
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Reference { get; set; }
        public string Owner { get; set; }
		
		public GeotechnicalInformation Geo { get; set; }
        
        public List<Tree> Trees { get; set; }
    }
}
