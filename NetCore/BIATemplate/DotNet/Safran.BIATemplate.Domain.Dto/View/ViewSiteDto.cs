// <copyright file="ViewSiteDto.cs" company="Safran">
// Copyright (c) Safran. All rights reserved.
// </copyright>

namespace Safran.BIATemplate.Domain.Dto.View
{
    /// <summary>
    /// ViewSite Dto.
    /// </summary>
    public class ViewSiteDto
    {
        /// <summary>
        /// Gets or sets the site identifier.
        /// </summary>
        public int SiteId { get; set; }

        /// <summary>
        /// Gets or sets the site.
        /// </summary>
        public string SiteTitle { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the view is the default one.
        /// </summary>
        public bool IsDefault { get; set; }
    }
}
