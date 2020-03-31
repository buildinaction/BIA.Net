// <copyright file="PlaneDto.cs" company="MyCompany">
//     Copyright (c) MyCompany. All rights reserved.
// </copyright>

namespace MyCompany.BIADemo.Domain.Dto.Plane
{
    using System;
    using MyCompany.BIADemo.Domain.Dto.Bia;

    /// <summary>
    /// The DTO used to represent a plane.
    /// </summary>
    public class PlaneDto : BaseDto
    {
        /// <summary>
        /// Gets or sets the Manufacturer's Serial Number.
        /// </summary>
        public string Msn { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the plane is active.
        /// </summary>
        public bool IsActive { get; set; }

        /// <summary>
        /// Gets or sets the first flight date only.
        /// </summary>
        public DateTime FirstFlightDate { get; set; }

        /// <summary>
        /// Gets or sets the first flight time only.
        /// </summary>
        public DateTime FirstFlightTime { get; set; }

        /// <summary>
        /// Gets or sets the last flight date and time.
        /// </summary>
        public DateTime? LastFlightDate { get; set; }

        /// <summary>
        /// Gets or sets the capacity.
        /// </summary>
        public int Capacity { get; set; }
    }
}