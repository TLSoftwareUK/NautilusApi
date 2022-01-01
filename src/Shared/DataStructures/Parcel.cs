using System;
using System.Collections.Generic;
using System.Text;

namespace TLS.Nautilus.Api.Shared.DataStructures
{
    public class Parcel
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public List<Plot> Plots { get; set; }

        public Parcel()
        {
            Id = Guid.NewGuid();
            Plots = new List<Plot>();
            Name = String.Empty;
        }
    }
}
