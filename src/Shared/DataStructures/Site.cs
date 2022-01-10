using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;

namespace TLS.Nautilus.Api.Shared.DataStructures
{    
    /// <summary>
    /// Represents a single site
    /// </summary>
    public class Site : ISite, IJsonOnDeserialized
    {
        public event EventHandler? OnSiteChanged;

        /// <inheritdoc/>        
        public Guid Id { get; set; }

        /// <inheritdoc/>
        public string Name { get; set; }

        /// <inheritdoc/>
        public string Reference { get; set; }

        /// <inheritdoc/>
        public string Owner { get; set; }

        /// <inheritdoc/>
        public bool Calculating { get; set; }

        /// <inheritdoc/>
        public bool Rendering { get; set; }

        /// <inheritdoc/>
        public GeotechnicalInformation Geo { get; set; }

        /// <summary>
        /// Collection of trees in the site
        /// </summary>
        public List<Tree> Trees { get; set; }
        
        /// <summary>
        /// Collection of Plot Definitions used in the site
        /// </summary>
        public List<PlotDefinition> Definitions { get; set; }

        /// <summary>
        /// Collection of plots on the site
        /// </summary>
        public List<Parcel> Parcels { get; set; }

        /// <inheritdoc/>
        IReadOnlyList<Tree> ISite.Trees => Trees;

        /// <inheritdoc/>
        IReadOnlyList<PlotDefinition> ISite.Definitions => Definitions;

        /// <inheritdoc/>
        IReadOnlyList<Parcel> ISite.Parcels => Parcels;

        public Site()
        {
            Name = String.Empty;
            Reference = String.Empty;
            Owner = String.Empty;
            Geo = new GeotechnicalInformation();
            Trees = new List<Tree>();
            Definitions = new List<PlotDefinition>();
            Parcels = new List<Parcel>();
        }

        public void SiteChanged()
        {
            OnSiteChanged?.Invoke(this, null);
        }

        public void OnDeserialized()
        {
            //TODO: Inflate members
            foreach (Parcel parcel in Parcels)
            {
                foreach (Plot p in parcel.Plots)
                {
                    var def = Definitions.Find(pd => pd.Id == p.DefinitionId);
                    if (def == null)
                        throw new InvalidOperationException("Plot Definition id not found");

                    p.Definition = def;
                }
            }            
        }

        /// <inheritdoc/>
        public PlotDefinition AddPlotDefinition(string definitionName)
        {
            PlotDefinition defintion = new PlotDefinition()
            { 
                PlotName = definitionName 
            };

            Definitions.Add(defintion);

            return defintion;
        }

        /// <inheritdoc/>
        public void RemovePlotDefintion(PlotDefinition plotDefinition)
        {
            //TODO: Handle plots using this definition
            Definitions.Remove(plotDefinition);
        }

        /// <inheritdoc/>
        public Parcel AddParcel()
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc/>
        public void RemoveParcel(Parcel parcel)
        {
            Parcels.Remove(parcel);
        }

        /// <inheritdoc/>
        public Plot? GetPlot(Guid id)
        {
            foreach (Parcel p in Parcels)
            {
                Plot? plot = p.Plots.Find(plot => plot.Id == id);
                if(plot != null)
                    return plot;
                    
            }

            return null;
        }
    }
}
