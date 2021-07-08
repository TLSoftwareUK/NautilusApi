using System;
using System.Collections.Generic;
using System.Text;

namespace TLS.Nautilus.Api.Shared.DataStructures
{
    public class PlotDefinition
    {
        public Guid Id { get; set; }
        
        public string PlotName { get; set; }

        public List<PlotNode> Nodes { get; set; }
        public List<PlotFoundation> Foundations { get; set; }

        public PlotDefinition()
        {
            Id = Guid.NewGuid();
            Nodes = new List<PlotNode>();
            Foundations = new List<PlotFoundation>();
        }
    }

    public class PlotNode
    {
        public Guid NodeId { get; set; }

        public double X { get; set; }
        public double Y { get; set; }
    }

    public class PlotFoundation
    {
        public Guid FoundationId { get; set; }

        public double LineLoad { get; set; }

        public Guid StartNode { get; set; }
        public Guid EndNode { get; set; }

    }
}
