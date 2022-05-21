using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace TLS.Nautilus.Api.Shared.DataStructures
{
    public class Plot
    {        
        public Guid Id { get; set; }

        public string PlotName { get; set; }

        public List<PlotNode> Nodes { get; set; }
        public List<PlotFoundation> Foundations { get; set; }

        public Guid DefinitionId { get; set; }

        public DoubleVector2 Location { get; set; }

        public double Bearing { get; set; }

        [JsonIgnore]
        public PlotDefinition? Definition
        {
            get
            {
                return _definition;
            }
            internal set
            {
                _definition = value;
                DefinitionId = value.Id;
            }
        }

        [JsonIgnore]
        public PlotDefinition? _definition;

        public Plot()
        {
            Id = Guid.NewGuid();
            Nodes = new List<PlotNode>();
            Foundations = new List<PlotFoundation>();
            PlotName = String.Empty;

        }
    }

    public class PlotFoundation
    {
        public Guid FoundationId { get; set; }

        public double LineLoad { get; set; }

        public double WallWidth { get; set; }

        public double RequiredWidth { get; set; }
        public double? WidthOverride { get; set; }

        public double TopOfConcreteLevel { get; set;}
        public double FormationLevel { get; set; }

        public Guid StartNode { get; set; }
        public DoubleVector2 StartNodeLocation { get; set; }

        public Guid EndNode { get; set; }
        public DoubleVector2 EndNodeLocation { get; set; }

        public double[] RequiredDepths { get; set; }

        public PlotFoundationAnalysis[] Analysis { get; set; }
    }   
    
    public class PlotFoundationAnalysis
    {
        public double ExistingGroundFormationLevel { get; set; }
        public double ProposedGroundFormationLevel { get; set; }        
        public Guid? DominantTree { get; set; }
    }
}
