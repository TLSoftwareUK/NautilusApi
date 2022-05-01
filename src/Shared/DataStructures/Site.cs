using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Numerics;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace TLS.Nautilus.Api.Shared.DataStructures
{
    /// <summary>
    /// Represents a single site
    /// </summary>
    public partial class Site : ObservableObject, ISite, IJsonOnDeserialized
    {
        public event EventHandler? OnRemoteSiteChanged;

        /// <inheritdoc/>        
        public Guid Id { get; set; }

        [ObservableProperty]
        private string _name;

        [ObservableProperty]
        private string _reference;

        [ObservableProperty]
        private string _owner;

        /// <inheritdoc/>
        public bool Calculating { get; set; }

        /// <inheritdoc/>
        public bool Rendering { get; set; }

        /// <inheritdoc/>
        public GeotechnicalInformation Geo
        {
            get
            {
                return _geo;
            }
            set
            {
                _geo = value;
                _geo.PropertyChanged += (args, e) => OnPropertyChanged(nameof(Geo));
            }
        }

        private GeotechnicalInformation _geo;

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

        /// <summary>
        /// Collection of plots on the site
        /// </summary>
        public List<JobResult> JobResults { get; set; }

        public List<string> Xrefs { get; set; }

        /// <inheritdoc/>
        IReadOnlyList<Tree> ISite.Trees => Trees;

        /// <inheritdoc/>
        IReadOnlyList<PlotDefinition> ISite.Definitions => Definitions;

        /// <inheritdoc/>
        IReadOnlyList<Parcel> ISite.Parcels => Parcels;

        /// <inheritdoc/>
        IReadOnlyList<JobResult> ISite.JobResults => JobResults;

        IReadOnlyList<string> ISite.Xrefs => Xrefs;

        public Site()
        {
            Name = String.Empty;
            Reference = String.Empty;
            Owner = String.Empty;
            Geo = new GeotechnicalInformation();
            Trees = new List<Tree>();
            Definitions = new List<PlotDefinition>();
            Parcels = new List<Parcel>();
            JobResults = new List<JobResult>();
            Xrefs = new List<string>();
        }

        public void SiteChanged()
        {
            OnRemoteSiteChanged?.Invoke(this, null);
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
            OnPropertyChanged(nameof(ISite.Definitions));

            return defintion;
        }

        /// <inheritdoc/>
        public void RemovePlotDefintion(PlotDefinition plotDefinition)
        {
            //TODO: Handle plots using this definition
            Definitions.Remove(plotDefinition);
            OnPropertyChanged(nameof(ISite.Definitions));
        }

        /// <inheritdoc/>
        public Parcel AddParcel(string parcelName)
        {
            Parcel parcel = new Parcel()
            {
                Name = parcelName
            };

            Parcels.Add(parcel);
            OnPropertyChanged(nameof(ISite.Parcels));

            return parcel;
        }

        /// <inheritdoc/>
        public void RemoveParcel(Parcel parcel)
        {
            Parcels.Remove(parcel);
            OnPropertyChanged(nameof(ISite.Parcels));
        }

        /// <inheritdoc/>
        public Plot? GetPlot(Guid id)
        {
            foreach (Parcel p in Parcels)
            {
                Plot? plot = p.Plots.Find(plot => plot.Id == id);
                if (plot != null)
                    return plot;

            }

            return null;
        }

        public Tree AddTree(string reference, Vector2 location, double height, TreeSpecies species, Phase phase)
        {
            Tree tree = new Tree()
            {
                TreeReference = reference,
                Location = location,
                Height = height,
                Species = species,
                Phase = phase
            };
            Trees.Add(tree);
            return tree;
        }

        public void RemoveTree(Tree tree)
        {
            Trees.Remove(tree);
        }

        public void AddJobResult(JobResult jobResult)
        {
            JobResults.Add(jobResult);
        }

        public void AddXref(string name)
        {
            Xrefs.Add(name);
        }
    }
}
