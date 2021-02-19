// <copyright file="AssignViewToSiteDto.cs" company="Safran">
// Copyright (c) Safran. All rights reserved.
// </copyright>

namespace Safran.BIADemo.Domain.Dto.View
{
    /// <summary>
    /// AssignViewToSite Dto.
    /// </summary>
    public class AssignViewToSiteDto
    {
        /// <summary>
        /// Gets or sets the view identifier.
        /// </summary>
        public int ViewId { get; set; }

        /// <summary>
        /// Gets or sets the site identifier.
        /// </summary>
        public int SiteId { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this instance is assign.
        /// </summary>
        public bool IsAssign { get; set; }
    }
}
