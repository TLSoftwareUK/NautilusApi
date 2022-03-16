using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Numerics;
using TLS.Nautilus.Api.Shared.DataStructures;

namespace TLS.Nautilus.Api.Shared
{
    /// <summary>
    /// Interface defining operation that can occur to a site
    /// </summary>
    public interface ISite : INotifyPropertyChanged
    {
        event EventHandler? OnRemoteSiteChanged;

        /// <summary>
        /// GUID indentifying this site
        /// </summary>
        Guid Id { get; }

        /// <summary>
        /// Name of project
        /// </summary>
        string Name { get; set; }

        /// <summary>
        /// Reference code supplied by owner
        /// </summary>
        string Reference { get; set; }

        /// <summary>
        /// Owner Id
        /// </summary>
        //TODO: Deprecate this, site doesnt need to know who owns it
        string Owner { get; set; }

        /// <summary>
        /// Indicates if a calculation has been queued
        /// </summary>
        bool Calculating { get; set; }

        /// <summary>
        /// Indicates if a render has been queued
        /// </summary>
        bool Rendering { get; set; }

        /// <summary>
        /// Geotechnicl Data on the site
        /// </summary>
        GeotechnicalInformation Geo { get; }

        /// <summary>
        /// Read-only collection of trees on the site
        /// </summary>
        IReadOnlyList<Tree> Trees { get; }

        /// <summary>
        /// Read-only collection of Plot Definitions used in the site
        /// </summary>
        IReadOnlyList<PlotDefinition> Definitions { get; }

        /// <summary>
        /// Read-only collection of plots on the site
        /// </summary>
        IReadOnlyList<Parcel> Parcels { get; }

        /// <summary>
        /// Add a new plot definition to the site
        /// </summary>
        /// <param name="definitionName">Name of new definition</param>
        /// <returns>New Plot Definition</returns>
        PlotDefinition AddPlotDefinition(string definitionName);

        /// <summary>
        /// Remove an existing Plot Definition from the site
        /// </summary>
        /// <param name="plotDefinition">Plot Definition to remove</param>
        void RemovePlotDefintion(PlotDefinition plotDefinition);

        /// <summary>
        /// Add a new parcel to the site
        /// </summary>
        /// <returns>New Parcel</returns>
        public Parcel AddParcel(string parcelName);

        /// <summary>
        /// Remove an existing Parcel from the site
        /// </summary>
        /// <param name="parcel">Parcel to remove</param>
        void RemoveParcel(Parcel parcel);

        /// <summary>
        /// Get plot by id
        /// </summary>
        /// <param name="id">Id of plot to return</param>
        /// <returns>Plot or null if not found</returns>
        Plot? GetPlot(Guid id);

        public Tree AddTree(string reference, Vector2 location, double height, string species, Phase phase);

        public void RemoveTree(Tree tree);
    }
}
