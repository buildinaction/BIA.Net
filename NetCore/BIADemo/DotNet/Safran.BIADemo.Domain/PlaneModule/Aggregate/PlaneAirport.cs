// BIADemo only
// <copyright file="PlaneAirport.cs" company="Safran">
//     Copyright (c) Safran. All rights reserved.
// </copyright>

namespace Safran.BIADemo.Domain.PlaneModule.Aggregate
{
    using BIA.Net.Core.Domain;

    /// <summary>
    /// The entity conformcertif repair site.
    /// </summary>
    public class PlaneAirport : VersionedTable
    {
        /// <summary>
        /// Gets or sets the conformcertif.
        /// </summary>
        public virtual Plane Plane { get; set; }

        /// <summary>
        /// Gets or sets the conformcertif id.
        /// </summary>
        public int PlaneId { get; set; }

        /// <summary>
        /// Gets or sets the site.
        /// </summary>
        public virtual Airport Airport { get; set; }

        /// <summary>
        /// Gets or sets the site id.
        /// </summary>
        public int AirportId { get; set; }
    }
}
