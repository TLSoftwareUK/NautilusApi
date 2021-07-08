using System;
using System.Collections.Generic;
using System.Text;

namespace TLS.Nautilus.Api.Shared.DataStructures
{
    public class Tree
    {
        public Guid Id { get; set; }

        public string TreeReference { get; set; }

        public string Species { get; set; }
        
        public double Height { get; set; }
        
        public Phase Phase { get; set; }
    }
    
    public enum WaterDemand
    {
        Low,
        Medium,
        High
    }

    public enum TreeType
    {
        Deciduous,
        Coniferous
    }

    public enum Phase
    {
        Proposed,
        Existing
    }
}
