// BIADemo only
// <copyright file="AirportDto.cs" company="Safran">
//     Copyright (c) Safran. All rights reserved.
// </copyright>

namespace Safran.BIADemo.Domain.Dto.Plane
{
    using BIA.Net.Core.Domain.Dto.Base;

    /// <summary>
    /// The DTO used to represent a airport.
    /// </summary>
    public class AirportDto : BaseDto
    {
        /// <summary>
        /// Gets or sets the name of the airport.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the City where is the airport.
        /// </summary>
        public string City { get; set; }
    }
}
