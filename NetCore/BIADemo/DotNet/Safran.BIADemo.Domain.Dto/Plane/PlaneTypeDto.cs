// BIADemo only
// <copyright file="PlaneTypeDto.cs" company="Safran">
//     Copyright (c) Safran. All rights reserved.
// </copyright>

namespace Safran.BIADemo.Domain.Dto.Plane
{
    using System;
    using BIA.Net.Core.Domain.Dto.Base;

    /// <summary>
    /// The DTO used to represent a plane.
    /// </summary>
    public class PlaneTypeDto : BaseDto
    {
        /// <summary>
        /// Gets or sets the Manufacturer's Serial Number.
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// Gets or sets the first flight date.
        /// </summary>
        public DateTime? CertificationDate { get; set; }
    }
}