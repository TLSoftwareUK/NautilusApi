using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace TLS.Nautilus.Api.Shared.DataStructures
{
    public class Tree
    {
        public Guid Id { get; set; }

        public string TreeReference { get; set; }

        public TreeSpecies Species { get; set; }
        
        public double Height { get; set; }
        
        public Phase Phase { get; set; }

        public DoubleVector2 Location { get; set; }

        public Tree()
        {
            Id = Guid.NewGuid();
            Species = TreeSpecies.Undefined;
            Location = DoubleVector2.Zero;
        }
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

    //TODO: Add other species
    public enum TreeSpecies
    {
        [Description("")] Undefined,
        [Description("English Elm")] EnglishElm,
        [Description("Wheatley Elm")] WheatleyElm,
        [Description("Wych Elm")] WychElm,
        [Description("Eucalyptus")] Eucalyptus,
        [Description("Hawthorn")] Hawthorn,
        [Description("English Oak")] EnglishOak,
        [Description("Holm Oak")] HolmOak,
        [Description("Red Oak")] RedOak,
        [Description("Turkey Oak")] TurkeyOak,
        [Description("Hybrid Black Poplar")] HybridBlackPoplar,
        [Description("Lombardy Poplar")] LombardyPoplar,
        [Description("White Poplar")] WhitePoplar,
        [Description("Crack Willow")] CrackWillow,
        [Description("Weeping Willow")] WeepingWillow,
        [Description("White Willow")] WhiteWillow
    }
}
