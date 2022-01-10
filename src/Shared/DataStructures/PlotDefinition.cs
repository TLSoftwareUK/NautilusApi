using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Numerics;
using System.Text.Json.Serialization;
using System.Linq;

namespace TLS.Nautilus.Api.Shared.DataStructures
{
    public partial class PlotDefinition : ObservableObject, IJsonOnDeserialized
    {
        public Guid Id { get; set; }
        
        public string PlotName { get; set; }

        public ObservableCollection<PlotNode> Nodes { get; set; }
        public ObservableCollection<PlotFoundationDefinition> Foundations { get; set; }

        public PlotDefinition()
        {
            Id = Guid.NewGuid();
            Nodes = new ObservableCollection<PlotNode>()
            {
                new PlotNode() { NodeId = Guid.NewGuid(), Location = new Vector2(0,0) }
            };
            Foundations = new ObservableCollection<PlotFoundationDefinition>();
            PlotName = String.Empty;
        }

        public void OnDeserialized()
        {
            foreach(PlotFoundationDefinition pfd in Foundations)
            {
                var start = Nodes.First(n => n.NodeId == pfd.StartNodeId);                
                if (start == null)
                    throw new InvalidOperationException("Start node id not found");

                var end = Nodes.First(n => n.NodeId == pfd.EndNodeId);
                if (end == null)
                    throw new InvalidOperationException("End node id not found");

                pfd.StartNode = start;
                pfd.EndNode = end;
            }
        }
    }

    public class PlotNode
    {
        public Guid NodeId { get; set; }

        public Vector2 Location { get; set; }
    }

    public partial class PlotFoundationDefinition : ObservableObject
    {
        public Guid FoundationId { get; set; }

        [ObservableProperty]
        private double _lineLoad;

        [ObservableProperty]
        private double _wallWidth;

        public Guid StartNodeId { get; private set; }
        public Guid EndNodeId { get; private set; }

        [JsonIgnore]
        public PlotNode StartNode { get { return _startNode; } set { _startNode = value; StartNodeId = value.NodeId; } }
        
        [JsonIgnore]
        public PlotNode EndNode { get { return _endNode; } set { _endNode = value; EndNodeId = value.NodeId; } }

        private PlotNode _startNode, _endNode;
    }
}
