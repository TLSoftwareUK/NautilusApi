using System;
using System.Collections.Generic;
using System.Numerics;

namespace TLS.Nautilus.Api.Shared.DataStructures
{
    public class PlotDefinition
    {
        public Guid Id { get; set; }
        
        public string PlotName { get; set; }

        public List<PlotNode> Nodes { get; set; }
        public List<PlotFoundationDefinition> Foundations { get; set; }

        public PlotDefinition()
        {
            Id = Guid.NewGuid();
            Nodes = new List<PlotNode>();
            Foundations = new List<PlotFoundationDefinition>();
            PlotName = String.Empty;
        }
    }

    public class PlotNode
    {
        public Guid NodeId { get; set; }

        public Vector2 Location { get; set; }
    }

    public class PlotFoundationDefinition
    {
        public Guid FoundationId { get; set; }

        public double LineLoad { get; set; }

        public double WallWidth { get; set; }

        public Guid StartNode { get; set; }
        public Guid EndNode { get; set; }
    }
}
